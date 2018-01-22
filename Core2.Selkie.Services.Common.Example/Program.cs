using System;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Core2.Selkie.EasyNetQ.Interfaces;
using Core2.Selkie.Services.Common.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Example
{
    [UsedImplicitly]
    internal class Program
    {
        private static void Main()
        {
            IWindsorContainer container = new WindsorContainer();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            container.Install(FromAssembly.Instance(executingAssembly));

            var client = container.Resolve <ISelkieManagementClient>();
            client.PurgeQueueForServiceAndMessage("Name",
                                                  "MessageName");

            var serviceConsole = container.Resolve <IServiceConsole>();
            serviceConsole.Start(false);

            container.Dispose();

            Console.ReadLine();
        }
    }
}