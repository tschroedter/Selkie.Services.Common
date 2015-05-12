using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Selkie.Services.Common.Tests.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class SelkieSleeperTests
    {
        [Test]
        public void SleepTest()
        {
            SelkieSleeper sut = new SelkieSleeper();

            Assert.DoesNotThrow(() => sut.Sleep(100));
        }
    }
}