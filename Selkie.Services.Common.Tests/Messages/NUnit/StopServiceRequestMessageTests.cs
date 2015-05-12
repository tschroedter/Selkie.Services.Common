using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Services.Common.Messages;

namespace Selkie.Services.Common.Tests.Messages.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class StopServiceRequestMessageTests
    {
        [Test]
        public void DefaultServiceNameTest()
        {
            StopServiceRequestMessage message = new StopServiceRequestMessage();

            Assert.AreEqual("Unknown",
                            message.ServiceName);
        }

        [Test]
        public void ServiceNameRoundtripTest()
        {
            StopServiceRequestMessage message = new StopServiceRequestMessage
                                                {
                                                    ServiceName = "Service Name"
                                                };

            Assert.AreEqual("Service Name",
                            message.ServiceName);
        }

        [Test]
        public void DefaultIsStopAllServicesTest()
        {
            StopServiceRequestMessage message = new StopServiceRequestMessage();

            Assert.False(message.IsStopAllServices);
        }

        [Test]
        public void IsStopAllServicesRoundtripTest()
        {
            StopServiceRequestMessage message = new StopServiceRequestMessage
                                                {
                                                    IsStopAllServices = true
                                                };

            Assert.True(message.IsStopAllServices);
        }
    }
}