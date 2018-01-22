using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Example
{
    [UsedImplicitly]
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

        public override bool IsAutoDetectAllowedForAssemblyName(string assemblyName)
        {
            return assemblyName.StartsWith("Core2.Selkie.",
                                           StringComparison.InvariantCulture);
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