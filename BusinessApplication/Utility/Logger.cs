namespace BusinessApplication.Utility
{
    public class Logger : ILogger
    {
        private List<ILoggingService> _loggingServices;

        public Logger()
        {
            _loggingServices = new List<ILoggingService>();
        }

        public Logger(List<ILoggingService> initialLoggingServices)
        {
            _loggingServices = initialLoggingServices;
        }

        public void AddLoggingService(ILoggingService loggingService)
        {
            _loggingServices.Add(loggingService);
        }

        public void RemoveLoggingService(ILoggingService loggingService)
        {
            _loggingServices.Remove(loggingService);
        }

        public void LogMessage(string message)
        {
            foreach (var loggingService in _loggingServices)
            {
                loggingService.LogMessage(message);
            }
        }

        public void LogError(string message)
        {
            foreach (var loggingService in _loggingServices)
            {
                loggingService.LogError(message);
            }
        }
    }
}
