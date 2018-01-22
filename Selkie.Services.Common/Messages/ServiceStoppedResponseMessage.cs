using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class ServiceStoppedResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}