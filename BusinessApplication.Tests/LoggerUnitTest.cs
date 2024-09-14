using BusinessApplication.Utility;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class LoggerUnitTest
    {
        [TestMethod]
        public void SingleLoggingServiceReceivesMessage()
        {
            var logger = new Logger();
            var loggingService = new LoggingServiceStub();
            logger.AddLoggingService(loggingService);
            var message = "msg";
            var error = "err";

            logger.LogMessage(message);
            logger.LogError(error);

            Assert.AreEqual(message, loggingService.MessageLog.FirstOrDefault());
            Assert.AreEqual(error, loggingService.ErrorLog.FirstOrDefault());
        }

        [TestMethod]
        public void MultipleLoggingServiceReceiveMessage()
        {
            var logger = new Logger();
            var loggingService1 = new LoggingServiceStub();
            var loggingService2 = new LoggingServiceStub();
            var loggingService3 = new LoggingServiceStub();

            logger.AddLoggingService(loggingService1);
            logger.AddLoggingService(loggingService2);
            logger.AddLoggingService(loggingService3);

            var message = "msg";
            var error = "err";

            logger.LogMessage(message);
            logger.LogError(error);

            Assert.AreEqual(message, loggingService1.MessageLog.FirstOrDefault());
            Assert.AreEqual(message, loggingService2.MessageLog.FirstOrDefault());
            Assert.AreEqual(message, loggingService3.MessageLog.FirstOrDefault());
            Assert.AreEqual(error, loggingService1.ErrorLog.FirstOrDefault());
            Assert.AreEqual(error, loggingService2.ErrorLog.FirstOrDefault());
            Assert.AreEqual(error, loggingService3.ErrorLog.FirstOrDefault());
        }
    }

    public class LoggingServiceStub : ILoggingService
    {
        public List<string> MessageLog = new List<string>();
        public List<string> ErrorLog = new List<string>();

        public void LogMessage(string message)
        {
            MessageLog.Add(message);
        }

        public void LogError(string message)
        {
            ErrorLog.Add(message);
        }
    }
}
