using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Services.Common.Messages;

namespace Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    [TestFixture]
    internal sealed class ServiceStoppedResponseMessageTests
    {
        private ServiceStoppedResponseMessage m_Message;

        [SetUp]
        public void Setup()
        {
            m_Message = new ServiceStoppedResponseMessage
                        {
                            ServiceName = "Test"
                        };
        }

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