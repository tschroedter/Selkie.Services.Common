﻿namespace Core2.Selkie.Services.Common
{
    public interface IServicesManager
    {
        bool IsAllServicesRunning { get; }
        int MaxTries { get; set; }
        void StopServices();
        void WaitForAllServices();
    }
}