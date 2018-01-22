using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Core2.Selkie.Services.Common.Tests.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class SelkieSleeperTests
    {
        [Test]
        public void SleepTest()
        {
            var sut = new SelkieSleeper();

            Assert.DoesNotThrow(() => sut.Sleep(100));
        }
    }
}