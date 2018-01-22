using System;
using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Services.Common.Messages;
using NUnit.Framework;

namespace Core2.Selkie.Services.Common.Tests.Messages.NUnit
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class PingRequestMessageTests
    {
        [SetUp]
        public void Setup()
        {
            m_Message = new PingRequestMessage
                        {
                            Request = DateTime.Now
                        };
        }

        private PingRequestMessage m_Message;

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