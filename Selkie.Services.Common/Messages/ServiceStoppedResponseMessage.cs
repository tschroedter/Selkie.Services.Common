using JetBrains.Annotations;

namespace Selkie.Services.Common.Messages
{
    public class ServiceStoppedResponseMessage
    {
        [NotNull]
        public string ServiceName = "Unknown";
    }
}