using BusinessApplication.Model;
using BusinessApplication.Repository;
using System.Linq.Expressions;
using System.Windows.Controls;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            var logger = new Logger();
            logger.AddLoggingService(new PopupLoggingService());
            var customerRepository = new Repository<Customer>(() => new AppDbContext(), App.AppLogger);
            DataContext = new CustomerViewModel(customerRepository);
        }
    }
}
