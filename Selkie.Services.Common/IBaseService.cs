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
        void Start();
        void Stop();
        void Initialize();
        event EventHandler ServiceStopped;
        void PurgeAllQueuesForService();
        void PurgeAllQueues();
        void PurgeQueuesRelatedToStoppingThisService();
    }
}