using BusinessApplication.ViewModel;
using System.Windows;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : Window
    {
        public ExportView(List<object> dataAsObjects)
        {
            InitializeComponent();
            DataContext = new ExportViewModel(dataAsObjects);
        }
    }
}
