using Core2.Selkie.EasyNetQ.Interfaces;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Services.Common.Messages;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesManager : IServicesManager
    {
        public ServicesManager([NotNull] ISelkieBus bus,
                               [NotNull] ISelkieLogger logger,
                               [NotNull] ISelkieSleeper sleeper)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Sleeper = sleeper;

            bus.SubscribeAsync <ServicesStatusResponseMessage>(GetType().ToString(),
                                                               ServicesStatusResponseHandler);
        }

        [UsedImplicitly]
        internal const int OneSecond = 1000;

        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private readonly ISelkieSleeper m_Sleeper;

        public void StopServices()
        {
            m_Logger.Info("Stopping 'Services'...");

            var message = new StopServiceRequestMessage();

            m_Bus.PublishAsync(message);
        }

        public void WaitForAllServices()
        {
            var tries = 0;

            while ( !IsAllServicesRunning &&
                    tries++ < MaxTries )
            {
                m_Logger.Info("Waiting for services to start...");
                m_Sleeper.Sleep(OneSecond);
            }
        }

        public bool IsAllServicesRunning { get; private set; }

        [UsedImplicitly]
        public int MaxTries { get; set; } = 10;

        [UsedImplicitly]
        internal void ServicesStatusResponseHandler([NotNull] ServicesStatusResponseMessage message)
        {
            IsAllServicesRunning = message.IsAllServicesRunning;
        }
    }
}