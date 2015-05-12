using JetBrains.Annotations;

namespace Selkie.Services.Common.Messages
{
    public class ServiceStartedResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}