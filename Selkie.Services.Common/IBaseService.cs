using System;
using JetBrains.Annotations;

namespace Selkie.Services.Common
{
    public interface IBaseService
    {
        [NotNull]
        string Name { get; }

        void Start();
        void Stop();
        void Initialize();
        event EventHandler ServiceStopped;
        void PurgeAllQueuesForService();
        void PurgeAllQueues();
        void PurgeQueuesRelatedToStoppingThisService();
    }
}