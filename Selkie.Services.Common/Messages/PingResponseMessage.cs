﻿using System;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Messages
{
    public class PingResponseMessage
    {
        public DateTime Request;
        public DateTime Response;

        [NotNull]
        public string ServiceName = "Unknown";
    }
}