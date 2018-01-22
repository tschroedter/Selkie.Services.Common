using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Core2.Selkie.Common.Interfaces;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;
using ISelkieConsole = Core2.Selkie.Services.Common.Interfaces.ISelkieConsole;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Singleton)]
    [UsedImplicitly]
    public sealed class ServiceConsole : IServiceConsole
    {
        public ServiceConsole([NotNull] ISelkieLogger logger,
                              [NotNull] ISelkieConsole console,
                              [NotNull] ISelkieEnvironment environment,
                              [NotNull] IService service)
        {
            m_Logger = logger;
            m_Console = console;
            m_Environment = environment;
            m_Service = service;

            m_Service.ServiceStopped += OnServiceStopped;
        }

        private readonly ISelkieConsole m_Console;
        private readonly ISelkieEnvironment m_Environment;
        private readonly ISelkieLogger m_Logger;
        private readonly IService m_Service;

        public void Start(bool isWaitForKey)
        {
            m_Service.Initialize();
            m_Service.Start();

            WaitForKeyOrRunForever(isWaitForKey);

            m_Service.Stop();
        }

        [UsedImplicitly]
        internal void OnServiceStopped([NotNull] object sender,
                                       [NotNull] EventArgs e)
        {
            m_Environment.Exit(0);
        }

        [ExcludeFromCodeCoverage]
        private void RunForever()
        {
            while ( true )
            {
                Thread.Sleep(30000);

                m_Logger.Info($"{m_Service.Name} is running...");
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void WaitForKeyOrRunForever(bool isWaitForKey)
        {
            if ( isWaitForKey )
            {
                WaitForReturnKey();
            }
            else
            {
                RunForever();
            }
        }

        private void WaitForReturnKey()
        {
            m_Console.WriteLine("Press 'Return' to exit!");
            m_Console.ReadLine();
        }
    }
}