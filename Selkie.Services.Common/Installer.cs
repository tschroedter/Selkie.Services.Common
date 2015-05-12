using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ.Management.Client;
using Selkie.Windsor;

namespace Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class Installer : BaseInstaller <Installer>
    {
        protected override void PreInstallComponents(IWindsorContainer container,
                                                     IConfigurationStore store)
        {
            base.PreInstallComponents(container,
                                      store);

            // ReSharper disable MaximumChainedReferences
            container.Register(Component.For <ManagementClient>()
                                        .UsingFactoryMethod(ManagementClientLoaderBuilder.CreateLoader)
                                        .LifestyleTransient());
            // ReSharper restore MaximumChainedReferences
        }
    }

    public class ManagementClientLoaderBuilder
    {
        private const string HttpLocalhost = "http://localhost";
        private const string Username = "selkieAdmin";
        private const string Password = "selkieAdmin";

        public static ManagementClient CreateLoader()
        {
            ManagementClient client = new ManagementClient(HttpLocalhost,
                                                           Username,
                                                           Password);
            return client;
        }
    }
}