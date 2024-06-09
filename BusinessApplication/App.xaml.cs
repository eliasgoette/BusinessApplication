using System.Windows;

namespace BusinessApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ILogger _appLogger = new Logger();

        public App()
        {
            _appLogger.AddLoggingService(new PopupLoggingService());
        }

        public static ILogger AppLogger { get { return _appLogger; } }
    }
}
