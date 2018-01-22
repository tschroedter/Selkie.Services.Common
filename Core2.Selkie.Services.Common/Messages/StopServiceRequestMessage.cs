using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class StopServiceRequestMessage
    {
        [UsedImplicitly]
        public bool IsStopAllServices;

        [NotNull]
        [UsedImplicitly]
        public string ServiceName = "Unknown";
    }
}