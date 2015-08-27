using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Common.Tests.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class ServicesManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            m_Bus = Substitute.For <ISelkieBus>();
            m_Logger = Substitute.For <ISelkieLogger>();
            m_Sleeper = Substitute.For <ISelkieSleeper>();

            m_Manager = new ServicesManager(m_Bus,
                                            m_Logger,
                                            m_Sleeper);
        }

        private ISelkieBus m_Bus;
        private ISelkieLogger m_Logger;
        private ServicesManager m_Manager;
        private ISelkieSleeper m_Sleeper;

        [Test]
        public void ConstructorSubscribesToPingRequestMessageTest()
        {
            m_Bus.Received().SubscribeAsync(m_Manager.GetType().ToString(),
                                            Arg.Any <Action <ServicesStatusResponseMessage>>());
        }

        [Test]
        public void IsAllServicesRunningDefaultTest()
        {
            Assert.False(m_Manager.IsAllServicesRunning);
        }

        [Test]
        public void MaxTriesDefaultTest()
        {
            Assert.AreEqual(10,
                            m_Manager.MaxTries);
        }

        [Test]
        public void MaxTriesRoundtripTest()
        {
            m_Manager.MaxTries = 5;

            Assert.AreEqual(5,
                            m_Manager.MaxTries);
        }

        [Test]
        public void ServicesStatusResponseHandlerTest()
        {
            var message = new ServicesStatusResponseMessage
                          {
                              IsAllServicesRunning = true
                          };

            m_Manager.ServicesStatusResponseHandler(message);

            Assert.AreEqual(message.IsAllServicesRunning,
                            m_Manager.IsAllServicesRunning);
        }

        [Test]
        public void StopServicesCallLoggerTest()
        {
            m_Manager.StopServices();

            m_Logger.Received().Info(Arg.Any <string>());
        }

        [Test]
        public void StopServicesSendsMessageTest()
        {
            m_Manager.StopServices();

            m_Bus.Received().PublishAsync(Arg.Any <StopServiceRequestMessage>());
        }

        [Test]
        public void WaitForAllServicesCallsLoggerTest()
        {
            m_Manager.MaxTries = 1;

            m_Manager.WaitForAllServices();

            m_Logger.Received().Info(Arg.Any <string>());
        }

        [Test]
        public void WaitForAllServicesCallsSleepTest()
        {
            m_Manager.MaxTries = 3;

            m_Manager.WaitForAllServices();

            m_Sleeper.Received(3).Sleep(ServicesManager.OneSecond);
        }

        [Test]
        public void WaitForAllServicesDoesNotCallsSleepForIsAllServicesRunningIsTrueTest()
        {
            var message = new ServicesStatusResponseMessage
                          {
                              IsAllServicesRunning = true
                          };
            m_Manager.ServicesStatusResponseHandler(message);
            m_Manager.MaxTries = 3;

            m_Manager.WaitForAllServices();

            m_Sleeper.DidNotReceive().Sleep(ServicesManager.OneSecond);
        }
    }
}