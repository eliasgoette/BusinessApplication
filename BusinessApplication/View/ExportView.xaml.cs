using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.ViewModel;
using System.Windows;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : Window
    {
        public ExportView()
        {
            InitializeComponent();
            var customerRepository = new Repository<Customer>(() => new AppDbContext(), App.AppLogger);
            DataContext = new ExportViewModel(customerRepository, App.AppLogger);
        }
    }
}
