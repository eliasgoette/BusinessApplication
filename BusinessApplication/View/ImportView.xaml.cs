using BusinessApplication.ViewModel;
using System.Windows;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        public ImportView()
        {
            InitializeComponent();
            DataContext = new ImportViewModel(App.AppLogger);
        }
    }
}
