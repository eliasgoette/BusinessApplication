using Autofac;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
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
            DataContext = new ExportViewModel(App.Container.Resolve<IRepository<Customer>>(), App.Container.Resolve<ILogger>(), new RelayCommand(this.Close));
        }
    }
}
