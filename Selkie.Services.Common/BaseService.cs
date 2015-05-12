using System;
using System.Linq;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Common.Messages;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Common
{
    public abstract class BaseService : IBaseService
    {
        private readonly IBus m_Bus;
        private readonly ISelkieManagementClient m_Client;
        private readonly ILogger m_Logger;
        private readonly string m_Name;

        protected BaseService([NotNull] IBus bus,
                              [NotNull] ILogger logger,
                              [NotNull] ISelkieManagementClient client,
                              [NotNull] string name)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Client = client;
            m_Name = name;
        }

        [NotNull]
        public IBus Bus
        {
            get
            {
                return m_Bus;
            }
        }

        [NotNull]
        public ILogger Logger
        {
            get
            {
                return m_Logger;
            }
        }

        public bool IsStopped { get; private set; }

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
            string name = GetType()
                .Name;
            Logger.Info("Purging all queues for '{0}'...".Inject(m_Name));
            m_Client.PurgeAllQueues(name);
        }

        public void PurgeQueuesRelatedToStoppingThisService()
        {
            m_Logger.Info("Purge all 'Stop Service' queues...");

            try
            {
                string name = m_Name.Split(' ')
                                    .First();

                m_Client.PurgeQueueForServiceAndMessage(name,
                                                        typeof ( StopServiceRequestMessage ).Name);
            }
            catch ( Exception exception ) // todo this is bad
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

        protected virtual void OnServiceStopped([NotNull] EventArgs e)
        {
            EventHandler handler = ServiceStopped;

            if ( handler != null )
            {
                handler(this,
                        e);
            }
        }

        private void SubscribeToServiceStopRequestMessage()
        {
            Bus.SubscribeHandlerAsync <StopServiceRequestMessage>(Logger,
                                                                  GetType()
                                                                      .ToString(),
                                                                  StopServiceRequestHandler);
        }

        private void SubscribeToPingRequestMessage()
        {
            Bus.SubscribeHandlerAsync <PingRequestMessage>(Logger,
                                                           GetType()
                                                               .ToString(),
                                                           PingRequestHandler);
        }

        internal void StopServiceRequestHandler([NotNull] StopServiceRequestMessage message)
        {
            m_Logger.Debug("Received StopServiceRequestMessage width IsStopAllServices = {0} ServiceName = {1}".Inject(message.IsStopAllServices,
                                                                                                                       message.ServiceName));

            if ( !IsMessageForMe(message) )
            {
                return;
            }

            IsStopped = true;

            Stop();

            StopServiceResponseMessage responseMessage = new StopServiceResponseMessage
                                                         {
                                                             ServiceName = Name
                                                         };
            m_Bus.Publish(responseMessage);

            OnServiceStopped(EventArgs.Empty);
        }

        protected virtual bool IsMessageForMe([NotNull] StopServiceRequestMessage message)
        {
            bool isMessageForMe = message.IsStopAllServices || message.ServiceName == Name;

            return isMessageForMe;
        }

        internal void PingRequestHandler([NotNull] PingRequestMessage message)
        {
            PingResponseMessage reply = new PingResponseMessage
                                        {
                                            ServiceName = m_Name,
                                            Request = message.Request
                                        };

            Bus.PublishAsync(reply);
        }

        protected abstract void ServiceStart();
        protected abstract void ServiceStop();
        protected abstract void ServiceInitialize();
    }
}