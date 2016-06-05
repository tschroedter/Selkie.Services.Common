using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Common
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
            m_Logger = container.Resolve <ISelkieLogger>();
            m_ServiceConsole = container.Resolve <IServiceConsole>();
        }

        [NotNull]
        public ISelkieLogger Logger
        {
            get
            {
                return m_Logger;
            }
        }

        private readonly IWindsorContainer m_Container;
        private readonly ISelkieLogger m_Logger;
        private readonly IServiceConsole m_ServiceConsole;

        public void Main(bool isWaitForKey)
        {
            m_ServiceConsole.Start(isWaitForKey);

            Release();
        }

        private void Release()
        {
            m_Container.Release(m_ServiceConsole);
            m_Container.Release(m_Logger);
        }
    }
}