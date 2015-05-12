using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Selkie.Services.Common.Example
{
    internal class Program
    {
        private static void Main()
        {
            IWindsorContainer container = new WindsorContainer();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            container.Install(FromAssembly.Instance(executingAssembly));

            ISelkieManagementClient client = container.Resolve <ISelkieManagementClient>();
            client.PurgeQueueForServiceAndMessage("Name",
                                                  "MessageName");

            IServiceConsole serviceConsole = container.Resolve <IServiceConsole>();
            serviceConsole.Start(false);

            container.Dispose();

            System.Console.ReadLine();
        }
    }
}