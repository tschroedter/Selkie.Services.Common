using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class StopServiceResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}