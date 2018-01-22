using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class ServiceProgram
    {
        public ServiceProgram([NotNull] IWindsorContainer container,
                              [NotNull] IWindsorInstaller installer)
        {
            m_Container = container;
            m_Container.Install(installer);
            Logger = container.Resolve <ISelkieLogger>();
            m_ServiceConsole = container.Resolve <IServiceConsole>();
        }

        [NotNull]
        [UsedImplicitly]
        public ISelkieLogger Logger { get; }

        private readonly IWindsorContainer m_Container;
        private readonly IServiceConsole m_ServiceConsole;

        public void Main(bool isWaitForKey)
        {
            m_ServiceConsole.Start(isWaitForKey);

            Release();
        }

        private void Release()
        {
            m_Container.Release(m_ServiceConsole);
            m_Container.Release(Logger);
        }
    }
}