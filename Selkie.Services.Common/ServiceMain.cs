using System;
using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using JetBrains.Annotations;
using NLog;

namespace Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class ServiceMain
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly IWindsorContainer Container = new WindsorContainer();

        public static void StartServiceAndWaitForKey([NotNull] IWindsorInstaller installer,
                                                     [NotNull] string serviceName)
        {
            StartService(installer,
                         serviceName,
                         true);
        }

        public static void StartServiceAndRunForever([NotNull] IWindsorInstaller installer,
                                                     [NotNull] string serviceName)
        {
            StartService(installer,
                         serviceName,
                         false);
        }

        private static void StartService([NotNull] IWindsorInstaller installer,
                                         [NotNull] string serviceName,
                                         bool isWaitForKey)
        {
            try
            {
                StartOnlyOneService(installer,
                                    serviceName,
                                    isWaitForKey);
            }
            catch ( TimeoutException )
            {
                Console.WriteLine("Process already running...shutting down!");
            }
            catch ( Exception exception )
            {
                LogException(exception);
            }
        }

        private static void StartOnlyOneService([NotNull] IWindsorInstaller installer,
                                                [NotNull] string serviceName,
                                                bool isWaitForKey)
        {
            using ( new OneServiceOnly(serviceName) )
            {
                var program = new ServiceProgram(Container,
                                                 installer);

                program.Main(isWaitForKey);
            }
        }

        private static void LogException([NotNull] Exception exception)
        {
            string message = "Big trouble..." + Environment.NewLine + exception;

            if ( Logger != null )
            {
                Logger.Error(message);
            }
            else
            {
                Console.WriteLine(message);
                Console.WriteLine("Press 'Return' to continue...");
                Console.ReadLine();
            }
        }
    }
}