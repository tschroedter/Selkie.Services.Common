using System;
using JetBrains.Annotations;
using Selkie.EasyNetQ;

namespace Selkie.Services.Common
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