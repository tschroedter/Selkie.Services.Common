using System;
using Core2.Selkie.EasyNetQ.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Interfaces
{
    public interface IBaseService
    {
        [NotNull]
        string Name { get; }

        [UsedImplicitly]
        ISelkieManagementClient ManagementClient { get; }

        void Initialize();

        [UsedImplicitly]
        void PurgeAllQueues();

        [UsedImplicitly]
        void PurgeAllQueuesForService();

        [UsedImplicitly]
        void PurgeQueuesRelatedToStoppingThisService();

        event EventHandler ServiceStopped;

        void Start();
        void Stop();
    }
}