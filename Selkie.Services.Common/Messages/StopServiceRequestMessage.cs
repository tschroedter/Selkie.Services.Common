using JetBrains.Annotations;

namespace Selkie.Services.Common.Messages
{
    public class StopServiceRequestMessage
    {
        public bool IsStopAllServices;

        [NotNull]
        public string ServiceName = "Unknown";
    }
}