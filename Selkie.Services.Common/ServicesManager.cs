using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesManager : IServicesManager
    {
        internal const int OneSecond = 1000;
        private readonly IBus m_Bus;
        private readonly ILogger m_Logger;
        private readonly ISelkieSleeper m_Sleeper;
        private int m_MaxTries = 10;

        public ServicesManager([NotNull] IBus bus,
                               [NotNull] ILogger logger,
                               [NotNull] ISelkieSleeper sleeper)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Sleeper = sleeper;

            bus.SubscribeHandlerAsync <ServicesStatusResponseMessage>(logger,
                                                                      GetType()
                                                                          .ToString(),
                                                                      ServicesStatusResponseHandler);
        }

        public void StopServices()
        {
            m_Logger.Info("Stopping 'Services'...");

            StopServiceRequestMessage message = new StopServiceRequestMessage();

            m_Bus.PublishAsync(message);
        }

        public void WaitForAllServices()
        {
            int tries = 0;

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