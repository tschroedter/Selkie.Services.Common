using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using JetBrains.Annotations;

namespace Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class ServiceProgram
    {
        private readonly IWindsorContainer m_Container;
        private readonly ILogger m_Logger;
        private readonly IServiceConsole m_ServiceConsole;

        public ServiceProgram([NotNull] IWindsorContainer container,
                              [NotNull] IWindsorInstaller installer)
        {
            m_Container = container;
            m_Container.Install(installer);
            m_Logger = container.Resolve <ILogger>();
            m_ServiceConsole = container.Resolve <IServiceConsole>();
        }

        [NotNull]
        public ILogger Logger
        {
            get
            {
                return m_Logger;
            }
        }

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