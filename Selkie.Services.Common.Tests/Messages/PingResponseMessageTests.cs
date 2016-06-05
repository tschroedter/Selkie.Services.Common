using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Services.Common.Messages;

namespace Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    [TestFixture]
    internal sealed class PingResponseMessageTests
    {
        [SetUp]
        public void Setup()
        {
            m_Request = new DateTime(2011,
                                     1,
                                     1);

            m_Message = new PingResponseMessage
                        {
                            ServiceName = "TestService",
                            Request = m_Request,
                            Response = DateTime.Now
                        };
        }

        private PingResponseMessage m_Message;
        private DateTime m_Request;

        [Test]
        public void ConstructorTest()
        {
            Assert.NotNull(m_Message);
        }

        [Test]
        public void RequestTest()
        {
            Assert.True(m_Message.Request == m_Request);
        }

        [Test]
        public void ResponseTest()
        {
            Assert.True(m_Message.Response.Millisecond > 0);
        }

        [Test]
        public void ServiceNameTest()
        {
            Assert.AreEqual("TestService",
                            m_Message.ServiceName);
        }
    }
}