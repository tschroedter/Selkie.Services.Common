using Core2.Selkie.EasyNetQ.Interfaces;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Example
{
    [ProjectComponent(Lifestyle.Singleton)]
    [UsedImplicitly]
    public class TestService
        : BaseService,
          IService
    {
        public TestService([NotNull] ISelkieBus bus,
                           [NotNull] ISelkieLogger logger,
                           [NotNull] ISelkieManagementClient client)
            : base(bus,
                   logger,
                   client,
                   "TestService")
        {
        }

        protected override void ServiceInitialize()
        {
            Logger.Info("ServiceInitialize()");
        }

        protected override void ServiceStart()
        {
            Logger.Info("ServiceStart()");
        }

        protected override void ServiceStop()
        {
            Logger.Info("ServiceStop()");
        }
    }
}