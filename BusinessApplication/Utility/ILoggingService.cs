﻿namespace BusinessApplication.Utility
{
    public interface ILoggingService
    {
        void LogMessage(string message);
        void LogError(string message);
    }
}
