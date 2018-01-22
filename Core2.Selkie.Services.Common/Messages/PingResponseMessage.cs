using System;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class PingResponseMessage
    {
        [UsedImplicitly]
        public DateTime Request;

        [UsedImplicitly]
        public DateTime Response;

        [NotNull]
        [UsedImplicitly]
        public string ServiceName = "Unknown";
    }
}