using System;
using System.Linq;
using JetBrains.Annotations;
using Core2.Selkie.Services.Common.Messages;
using Selkie.EasyNetQ;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Core2.Selkie.Services.Common
{
    public abstract class BaseService : IBaseService
    {
        protected BaseService([NotNull] ISelkieBus bus,
                              [NotNull] ISelkieLogger logger,
                              [NotNull] ISelkieManagementClient client,
                              [NotNull] string name)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Client = client;
            m_Name = name;
        }

        [NotNull]
        public ISelkieBus Bus
        {
            get
            {
                return m_Bus;
            }
        }

        [NotNull]
        public ISelkieLogger Logger
        {
            get
            {
                return m_Logger;
            }
        }

        [UsedImplicitly]
        public bool IsStopped { get; private set; }

        private readonly ISelkieBus m_Bus;
        private readonly ISelkieManagementClient m_Client;
        private readonly ISelkieLogger m_Logger;
        private readonly string m_Name;

        [NotNull]
        public ISelkieManagementClient ManagementClient
        {
            get
            {
                return m_Client;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public event EventHandler ServiceStopped;

        public void Initialize()
        {
            m_Logger.Info("Initializing service...");
            PurgeQueuesRelatedToStoppingThisService();
            SubscribeToServiceStopRequestMessage();
            SubscribeToPingRequestMessage();
            ServiceInitialize();
            m_Logger.Info("...service initialized!");
        }

        public void Start()
        {
            m_Logger.Info("Service: '{0}'...".Inject(GetType()));
            m_Logger.Info("Starting service...");
            ServiceStart();
            m_Logger.Info("...service started!");
        }

        public void Stop()
        {
            m_Logger.Info("Stopping service...");
            ServiceStop();
            m_Logger.Info("...service stopped!");
        }

        public void PurgeAllQueuesForService()
        {
            string name = GetType().Name;
            Logger.Info("Purging all queues for '{0}'...".Inject(m_Name));
            m_Client.PurgeAllQueues(name);
        }

        public void PurgeQueuesRelatedToStoppingThisService()
        {
            m_Logger.Info("Purge all 'Stop Service' queues...");

            try
            {
                string name = m_Name.Split(' ').First();

                m_Client.PurgeQueueForServiceAndMessage(name,
                                                        typeof( StopServiceRequestMessage ).Name);
            }
            catch ( Exception exception )
            {
                m_Logger.Error("Couldn't purge all 'Stop Service' queues!",
                               exception);
            }
        }

        public void PurgeAllQueues()
        {
            Logger.Info("Purging all queues...");
            m_Client.PurgeAllQueues();
        }

        protected virtual bool IsMessageForMe([NotNull] StopServiceRequestMessage message)
        {
            bool isMessageForMe = message.IsStopAllServices || message.ServiceName == Name;

            return isMessageForMe;
        }

        protected virtual void OnServiceStopped([NotNull] EventArgs e)
        {
            EventHandler handler = ServiceStopped;

            if ( handler != null )
            {
                handler(this,
                        e);
            }
        }

        protected abstract void ServiceInitialize();

        protected abstract void ServiceStart();
        protected abstract void ServiceStop();

        internal void PingRequestHandler([NotNull] PingRequestMessage message)
        {
            var reply = new PingResponseMessage
                        {
                            ServiceName = m_Name,
                            Request = message.Request
                        };

            Bus.PublishAsync(reply);
        }

        internal void StopServiceRequestHandler([NotNull] StopServiceRequestMessage message)
        {
            string inject = "Received StopServiceRequestMessage width " +
                            "IsStopAllServices = {0} ServiceName = {1}".Inject(message.IsStopAllServices,
                                                                               message.ServiceName);

            m_Logger.Debug(inject);

            if ( !IsMessageForMe(message) )
            {
                return;
            }

            IsStopped = true;

            Stop();

            var responseMessage = new StopServiceResponseMessage
                                  {
                                      ServiceName = Name
                                  };
            m_Bus.Publish(responseMessage);

            OnServiceStopped(EventArgs.Empty);
        }

        private void SubscribeToPingRequestMessage()
        {
            Bus.SubscribeAsync <PingRequestMessage>(GetType().ToString(),
                                                    PingRequestHandler);
        }

        private void SubscribeToServiceStopRequestMessage()
        {
            Bus.SubscribeAsync <StopServiceRequestMessage>(GetType().ToString(),
                                                           StopServiceRequestHandler);
        }
    }
}