using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class ServiceStartedResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}