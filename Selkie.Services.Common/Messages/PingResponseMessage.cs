using System;
using JetBrains.Annotations;

namespace Selkie.Services.Common.Messages
{
    public class PingResponseMessage
    {
        public DateTime Request;
        public DateTime Response;

        [NotNull]
        public string ServiceName = "Unknown";
    }
}