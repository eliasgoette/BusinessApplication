using BusinessApplication.Model;
using BusinessApplication.Repository;
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
            var customerRepository = new Repository<Customer>(() => new AppDbContext(), App.AppLogger);
            DataContext = new ImportViewModel(App.AppLogger, customerRepository);
        }
    }
}
