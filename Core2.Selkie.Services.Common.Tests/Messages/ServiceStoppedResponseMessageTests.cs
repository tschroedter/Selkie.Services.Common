using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Services.Common.Messages;
using NUnit.Framework;

namespace Core2.Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class ServiceStoppedResponseMessageTests
    {
        [SetUp]
        public void Setup()
        {
            m_Message = new ServiceStoppedResponseMessage
                        {
                            ServiceName = "Test"
                        };
        }

        private ServiceStoppedResponseMessage m_Message;

        [Test]
        public void ServiceNameDefaultTest()
        {
            Assert.AreEqual("Test",
                            m_Message.ServiceName);
        }

        [Test]
        public void ServiceNameRoundtripTest()
        {
            m_Message.ServiceName = "NewTest";

            Assert.AreEqual("NewTest",
                            m_Message.ServiceName);
        }
    }
}