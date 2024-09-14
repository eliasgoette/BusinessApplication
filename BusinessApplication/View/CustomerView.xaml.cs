using Autofac;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(App.Container.Resolve<IRepository<Customer>>(), App.Container.Resolve<IRepository<Address>>(), App.Container.Resolve<ILogger>());
        }
    }
}
