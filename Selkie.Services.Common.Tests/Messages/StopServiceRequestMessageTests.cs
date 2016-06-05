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
        public void DefaultIsStopAllServicesTest()
        {
            var message = new StopServiceRequestMessage();

            Assert.False(message.IsStopAllServices);
        }

        [Test]
        public void DefaultServiceNameTest()
        {
            var message = new StopServiceRequestMessage();

            Assert.AreEqual("Unknown",
                            message.ServiceName);
        }

        [Test]
        public void IsStopAllServicesRoundtripTest()
        {
            var message = new StopServiceRequestMessage
                          {
                              IsStopAllServices = true
                          };

            Assert.True(message.IsStopAllServices);
        }

        [Test]
        public void ServiceNameRoundtripTest()
        {
            var message = new StopServiceRequestMessage
                          {
                              ServiceName = "Service Name"
                          };

            Assert.AreEqual("Service Name",
                            message.ServiceName);
        }
    }
}