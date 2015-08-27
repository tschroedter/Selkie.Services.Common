using System;
using System.Threading;
using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Singleton)]
    public sealed class ServiceConsole : IServiceConsole
    {
        private readonly ISelkieConsole m_Console;
        private readonly ISelkieEnvironment m_Environment;
        private readonly ISelkieLogger m_Logger;
        private readonly IService m_Service;

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

        public void Start(bool isWaitForKey)
        {
            m_Service.Initialize();
            m_Service.Start();

            WaitForKeyOrRunForever(isWaitForKey);

            m_Service.Stop();
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

        //ncrunch: no coverage start
        private void RunForever()
        {
            while ( true )
            {
                Thread.Sleep(30000);

                m_Logger.Info("{0} is running...".Inject(m_Service.Name));
            }
            // ReSharper disable once FunctionNeverReturns
        }

        //ncrunch: no coverage end

        internal void OnServiceStopped([NotNull] object sender,
                                       [NotNull] EventArgs e)
        {
            m_Environment.Exit(0);
        }

        private void WaitForReturnKey()
        {
            m_Console.WriteLine("Press 'Return' to exit!");
            m_Console.ReadLine();
        }
    }
}