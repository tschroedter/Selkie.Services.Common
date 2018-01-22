using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.EasyNetQ;
using Core2.Selkie.Services.Common.Messages;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Services.Common.Tests.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    // ReSharper disable once ClassTooBig
    internal sealed class BaseServiceTests
    {
        [SetUp]
        public void Setup()
        {
            m_Bus = Substitute.For <ISelkieBus>();
            m_Logger = Substitute.For <ISelkieLogger>();
            m_Client = Substitute.For <ISelkieManagementClient>();

            m_Service = new TestBaseService(m_Bus,
                                            m_Logger,
                                            m_Client);
        }

        private ISelkieBus m_Bus;
        private ISelkieManagementClient m_Client;
        private ISelkieLogger m_Logger;
        private TestBaseService m_Service;
        private bool m_WasRaisedServiceStopped;

        private void OnServiceStopped([NotNull] object sender,
                                      [NotNull] EventArgs e)
        {
            m_WasRaisedServiceStopped = true;
        }

        [NotNull]
        private static StopServiceRequestMessage CreateMessage()
        {
            var message = new StopServiceRequestMessage
                          {
                              IsStopAllServices = true
                          };

            return message;
        }

        private class TestBaseService : BaseService
        {
            public TestBaseService([NotNull] ISelkieBus bus,
                                   [NotNull] ISelkieLogger logger,
                                   [NotNull] ISelkieManagementClient client)
                : base(bus,
                       logger,
                       client,
                       typeof( TestBaseService ).FullName)
            {
            }

            public bool WasCalledServiceInitialize { get; private set; }
            public bool WasCalledServiceStart { get; private set; }
            public bool WasCalledServiceStop { get; private set; }

            protected override void ServiceInitialize()
            {
                WasCalledServiceInitialize = true;
            }

            protected override void ServiceStart()
            {
                WasCalledServiceStart = true;
            }

            protected override void ServiceStop()
            {
                WasCalledServiceStop = true;
            }
        }

        [Test]
        public void BusDefaultTest()
        {
            Assert.AreEqual(m_Bus,
                            m_Service.Bus);
        }

        [Test]
        public void InitializeCallsPurgeQueuesRelatedToStoppingServicesTest()
        {
            string expectedName = typeof( TestBaseService ).FullName;
            string expectedMessageName = typeof( StopServiceRequestMessage ).Name;

            m_Service.Initialize();

            m_Client.Received()
                    .PurgeQueueForServiceAndMessage(Arg.Is <string>(x => x == expectedName),
                                                    Arg.Is <string>(x => x == expectedMessageName));
        }

        [Test]
        public void InitializeCallsServiceInitializeTest()
        {
            m_Service.Initialize();

            Assert.True(m_Service.WasCalledServiceInitialize);
        }

        [Test]
        public void InitializeSubscribesToPingRequestMessageTest()
        {
            m_Service.Initialize();

            string subscriptionId = m_Service.GetType()
                                             .ToString();

            m_Bus.Received()
                 .SubscribeAsync(subscriptionId,
                                 Arg.Any <Action <PingRequestMessage>>());
        }

        [Test]
        public void InitializeSubscribesToStopServiceRequestMessageTest()
        {
            m_Service.Initialize();

            string subscriptionId = m_Service.GetType()
                                             .ToString();

            m_Bus.Received()
                 .SubscribeAsync(subscriptionId,
                                 Arg.Any <Action <StopServiceRequestMessage>>());
        }

        [Test]
        public void IsStoppedDefaultTest()
        {
            Assert.False(m_Service.IsStopped);
        }

        [Test]
        public void LoggerDefaultTest()
        {
            Assert.AreEqual(m_Logger,
                            m_Service.Logger);
        }

        [Test]
        public void ManagementClientDefaultTest()
        {
            Assert.AreEqual(m_Client,
                            m_Service.ManagementClient);
        }

        [Test]
        public void NameTest()
        {
            string expected = typeof( TestBaseService ).FullName;
            string actual = m_Service.Name;

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void PingRequestHandlerSendsReplyTest()
        {
            var request = new PingRequestMessage();

            m_Service.PingRequestHandler(request);

            m_Bus.Received()
                 .PublishAsync(Arg.Is <PingResponseMessage>(x => x.Request == request.Request));
        }

        [Test]
        public void PurgeAllQueuesCallsClientTest()
        {
            m_Service.PurgeAllQueues();

            m_Client.Received()
                    .PurgeAllQueues();
        }

        [Test]
        public void PurgeAllQueuesCallsLoggerTest()
        {
            m_Service.PurgeAllQueues();

            m_Logger.Received()
                    .Info(Arg.Is <string>(x => x.StartsWith("Purging all")));
        }

        [Test]
        public void PurgeAllQueuesForServiceCallsClientTest()
        {
            m_Service.PurgeAllQueuesForService();

            string name = typeof( TestBaseService ).Name;

            m_Client.Received()
                    .PurgeAllQueues(Arg.Is <string>(x => x == name));
        }

        [Test]
        public void PurgeAllQueuesForServiceCallsLoggerTest()
        {
            m_Service.PurgeAllQueuesForService();

            m_Logger.Received()
                    .Info(Arg.Is <string>(x => x.StartsWith("Purging all")));
        }

        [Test]
        public void PurgeQueuesRelatedToStoppingServicesCallsClientTest()
        {
            string expectedName = typeof( TestBaseService ).FullName;
            string expectedMessageName = typeof( StopServiceRequestMessage ).Name;

            m_Service.PurgeQueuesRelatedToStoppingThisService();

            m_Client.Received()
                    .PurgeQueueForServiceAndMessage(Arg.Is <string>(x => x == expectedName),
                                                    Arg.Is <string>(x => x == expectedMessageName));
        }

        [Test]
        public void StartCallsServiceStartTest()
        {
            m_Service.Start();

            Assert.True(m_Service.WasCalledServiceStart);
        }

        [Test]
        public void StopCallsServiceStopTest()
        {
            m_Service.Stop();

            Assert.True(m_Service.WasCalledServiceStop);
        }

        [Test]
        public void StopServicesRequestHandlerDoesNotStopsServiceForIsStopAllServicesFalseAndMatchingServiceNameTest()
        {
            var message = new StopServiceRequestMessage
                          {
                              IsStopAllServices = false,
                              ServiceName = "Unknown"
                          };

            m_Service.StopServiceRequestHandler(message);

            Assert.False(m_Service.IsStopped);
        }

        [Test]
        public void StopServicesRequestHandlerRaisesEventTest()
        {
            m_Service.ServiceStopped += OnServiceStopped;

            StopServiceRequestMessage message = CreateMessage();

            m_Service.StopServiceRequestHandler(message);

            Assert.True(m_WasRaisedServiceStopped);
        }

        [Test]
        public void StopServicesRequestHandlerSendsMessageTest()
        {
            StopServiceRequestMessage message = CreateMessage();

            m_Service.StopServiceRequestHandler(message);

            m_Bus.Received()
                 .Publish(Arg.Is <StopServiceResponseMessage>(x => x.ServiceName == typeof( TestBaseService ).FullName));
        }

        [Test]
        public void StopServicesRequestHandlerSetsIsStoppedTest()
        {
            m_Service.ServiceStopped += OnServiceStopped;

            StopServiceRequestMessage message = CreateMessage();

            m_Service.StopServiceRequestHandler(message);

            Assert.True(m_Service.IsStopped);
        }

        [Test]
        public void StopServicesRequestHandlerStopsServiceForIsStopAllServicesFalseAndMatchingServiceNameTest()
        {
            var message = new StopServiceRequestMessage
                          {
                              IsStopAllServices = false,
                              ServiceName = typeof( TestBaseService ).FullName
                          };

            m_Service.StopServiceRequestHandler(message);

            Assert.True(m_Service.IsStopped);
        }

        [Test]
        public void StopServicesRequestHandlerStopsServiceForIsStopAllServicesTrueTest()
        {
            StopServiceRequestMessage message = CreateMessage();

            m_Service.StopServiceRequestHandler(message);

            Assert.True(m_Service.IsStopped);
        }
    }
}