using System;
using System.Linq;
using Core2.Selkie.EasyNetQ.Interfaces;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Services.Common.Messages;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    public abstract class BaseService : IBaseService
    {
        protected BaseService([NotNull] ISelkieBus bus,
                              [NotNull] ISelkieLogger logger,
                              [NotNull] ISelkieManagementClient client,
                              [NotNull] string name)
        {
            Bus = bus;
            Logger = logger;
            ManagementClient = client;
            Name = name;
        }

        [NotNull]
        [UsedImplicitly]
        public ISelkieBus Bus { get; }

        [NotNull]
        [UsedImplicitly]
        public ISelkieLogger Logger { get; }

        [UsedImplicitly]
        public bool IsStopped { get; private set; }

        [NotNull]
        public ISelkieManagementClient ManagementClient { get; }

        public string Name { get; }

        public event EventHandler ServiceStopped;

        public void Initialize()
        {
            Logger.Info("Initializing service...");
            PurgeQueuesRelatedToStoppingThisService();
            SubscribeToServiceStopRequestMessage();
            SubscribeToPingRequestMessage();
            ServiceInitialize();
            Logger.Info("...service initialized!");
        }

        public void Start()
        {
            Logger.Info($"Service: '{GetType()}'...");
            Logger.Info("Starting service...");
            ServiceStart();
            Logger.Info("...service started!");
        }

        public void Stop()
        {
            Logger.Info("Stopping service...");
            ServiceStop();
            Logger.Info("...service stopped!");
        }

        public void PurgeAllQueuesForService()
        {
            string name = GetType().Name;
            Logger.Info($"Purging all queues for '{Name}'...");
            ManagementClient.PurgeAllQueues(name);
        }

        public void PurgeQueuesRelatedToStoppingThisService()
        {
            Logger.Info("Purge all 'Stop Service' queues...");

            try
            {
                string name = Name.Split(' ').First();

                ManagementClient.PurgeQueueForServiceAndMessage(name,
                                                                typeof( StopServiceRequestMessage ).Name);
            }
            catch ( Exception exception )
            {
                Logger.Error("Couldn't purge all 'Stop Service' queues!",
                             exception);
            }
        }

        public void PurgeAllQueues()
        {
            Logger.Info("Purging all queues...");
            ManagementClient.PurgeAllQueues();
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual bool IsMessageForMe([NotNull] StopServiceRequestMessage message)
        {
            bool isMessageForMe = message.IsStopAllServices || message.ServiceName == Name;

            return isMessageForMe;
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void OnServiceStopped([NotNull] EventArgs e)
        {
            EventHandler handler = ServiceStopped;

            handler?.Invoke(this,
                            e);
        }

        protected abstract void ServiceInitialize();

        protected abstract void ServiceStart();
        protected abstract void ServiceStop();

        [UsedImplicitly]
        internal void PingRequestHandler([NotNull] PingRequestMessage message)
        {
            var reply = new PingResponseMessage
                        {
                            ServiceName = Name,
                            Request = message.Request
                        };

            Bus.PublishAsync(reply);
        }

        [UsedImplicitly]
        internal void StopServiceRequestHandler([NotNull] StopServiceRequestMessage message)
        {
            string inject = "Received StopServiceRequestMessage width " +
                            $"IsStopAllServices = {message.IsStopAllServices} ServiceName = {message.ServiceName}";

            Logger.Debug(inject);

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
            Bus.Publish(responseMessage);

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