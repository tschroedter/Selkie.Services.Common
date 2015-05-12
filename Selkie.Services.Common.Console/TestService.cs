using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Common.Example
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class TestService : BaseService,
                               IService
    {
        public TestService([NotNull] IBus bus,
                           [NotNull] ILogger logger,
                           [NotNull] ISelkieManagementClient client)
            : base(bus,
                   logger,
                   client,
                   "TestService")
        {
        }

        protected override void ServiceStart()
        {
            Logger.Info("ServiceStart()");
        }

        protected override void ServiceStop()
        {
            Logger.Info("ServiceStop()");
        }

        protected override void ServiceInitialize()
        {
            Logger.Info("ServiceInitialize()");
        }
    }
}