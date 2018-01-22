using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Core2.Selkie.Services.Common.Messages;

namespace Core2.Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
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