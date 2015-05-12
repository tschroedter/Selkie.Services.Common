using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Services.Common.Messages;

namespace Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    [TestFixture]
    internal sealed class PingRequestMessageTests
    {
        private PingRequestMessage m_Message;

        [SetUp]
        public void Setup()
        {
            m_Message = new PingRequestMessage
                        {
                            Request = DateTime.Now
                        };
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.NotNull(m_Message);
        }

        [Test]
        public void RequestTest()
        {
            Assert.True(m_Message.Request.Millisecond > 0);
        }
    }
}