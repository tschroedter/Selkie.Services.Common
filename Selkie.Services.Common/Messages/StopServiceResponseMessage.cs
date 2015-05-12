using JetBrains.Annotations;

namespace Selkie.Services.Common.Messages
{
    public class StopServiceResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}