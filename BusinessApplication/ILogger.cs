namespace BusinessApplication
{
    public interface ILogger
    {
        void AddLoggingService(ILoggingService loggingService);

        void RemoveLoggingService(ILoggingService loggingService);

        void LogMessage(string message);

        void LogError(string message);
    }
}
