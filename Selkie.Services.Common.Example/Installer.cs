using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Services.Common.Example
{
    public class Installer
        : BasicConsoleInstaller,
          IWindsorInstaller
    {
        public new void Install(IWindsorContainer container,
                                IConfigurationStore store)
        {
            base.Install(container,
                         store);

            container.Register(Component.For <IService>().ImplementedBy <TestService>().LifeStyle.Transient);
        }

        public override string GetPrefixOfDllsToInstall()
        {
            return "Selkie.";
        }

        protected override void InstallComponents(IWindsorContainer container,
                                                  IConfigurationStore store)
        {
            base.InstallComponents(container,
                                   store);

            container.Register(Component.For <IService>().ImplementedBy <TestService>().LifeStyle.Transient);
        }
    }
}