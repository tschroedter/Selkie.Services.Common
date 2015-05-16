using System;
using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Selkie.Services.Common.Tests.NUnit
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class ServiceConsoleTests
    {
        [SetUp]
        public void Setup()
        {
            m_Logger = Substitute.For <ILogger>();
            m_Service = Substitute.For <IService>();
            m_Environment = Substitute.For <ISelkieEnvironment>();
            m_Console = Substitute.For <ISelkieConsole>();

            m_ServiceConsole = new ServiceConsole(m_Logger,
                                                  m_Console,
                                                  m_Environment,
                                                  m_Service);
        }

        private ISelkieConsole m_Console;
        private ISelkieEnvironment m_Environment;
        private ILogger m_Logger;
        private IService m_Service;
        private ServiceConsole m_ServiceConsole;

        [Test]
        public void ListensToServiceStoppedEventTest()
        {
            m_Service.Received().ServiceStopped += m_ServiceConsole.OnServiceStopped;
        }

        [Test]
        public void OnServiceStoppedCallsExitTest()
        {
            m_ServiceConsole.OnServiceStopped(this,
                                              EventArgs.Empty);

            m_Environment.Received().Exit(0);
        }

        [Test]
        public void RunCallsInitializeTest()
        {
            m_ServiceConsole.Start(true);

            m_Service.Received().Initialize();
        }

        [Test]
        public void RunCallsStartTest()
        {
            m_ServiceConsole.Start(true);

            m_Service.Received().Start();
        }

        [Test]
        public void RunCallsStopTest()
        {
            m_ServiceConsole.Start(true);

            m_Service.Received().Stop();
        }
    }
}