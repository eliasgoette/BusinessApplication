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
            var customerRepository = new Repository<Customer>(() => new AppDbContext());
            DataContext = new CustomerViewModel(customerRepository);
        }
    }
}
