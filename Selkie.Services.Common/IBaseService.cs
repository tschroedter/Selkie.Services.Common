using System;
using JetBrains.Annotations;
using Core2.Selkie.EasyNetQ;
using Selkie.EasyNetQ;

namespace Core2.Selkie.Services.Common
{
    public interface IBaseService
    {
        [NotNull]
        string Name { get; }

        ISelkieManagementClient ManagementClient { get; }
        void Initialize();
        void PurgeAllQueues();
        void PurgeAllQueuesForService();
        void PurgeQueuesRelatedToStoppingThisService();
        event EventHandler ServiceStopped;
        void Start();
        void Stop();
    }
}