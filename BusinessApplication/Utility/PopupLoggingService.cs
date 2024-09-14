using System.Windows;

namespace BusinessApplication.Utility
{
    public class PopupLoggingService : ILoggingService
    {
        public void LogMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LogError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
