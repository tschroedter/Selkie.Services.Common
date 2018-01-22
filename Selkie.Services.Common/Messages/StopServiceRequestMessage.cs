using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class StopServiceRequestMessage
    {
        public bool IsStopAllServices;

        [NotNull]
        public string ServiceName = "Unknown";
    }
}