using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Interfaces
{
    public interface IServicesManager
    {
        [UsedImplicitly]
        bool IsAllServicesRunning { get; }

        [UsedImplicitly]
        int MaxTries { get; set; }

        [UsedImplicitly]
        void StopServices();

        [UsedImplicitly]
        void WaitForAllServices();
    }
}