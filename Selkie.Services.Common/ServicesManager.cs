using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Common
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

        internal const int OneSecond = 1000;
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private readonly ISelkieSleeper m_Sleeper;
        private int m_MaxTries = 10;

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
                    tries++ < m_MaxTries )
            {
                m_Logger.Info("Waiting for services to start...");
                m_Sleeper.Sleep(OneSecond);
            }
        }

        public bool IsAllServicesRunning { get; private set; }

        public int MaxTries
        {
            get
            {
                return m_MaxTries;
            }
            set
            {
                m_MaxTries = value;
            }
        }

        internal void ServicesStatusResponseHandler([NotNull] ServicesStatusResponseMessage message)
        {
            IsAllServicesRunning = message.IsAllServicesRunning;
        }
    }
}