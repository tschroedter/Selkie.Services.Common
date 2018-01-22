﻿using JetBrains.Annotations;
using Core2.Selkie.EasyNetQ;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Services.Common.Example
{
    [ProjectComponent(Lifestyle.Singleton)]
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