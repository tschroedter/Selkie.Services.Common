using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    [UsedImplicitly]
    public class ServiceStoppedResponseMessage
    {
        [NotNull]
        [UsedImplicitly]
        public string ServiceName = "Unknown";
    }
}