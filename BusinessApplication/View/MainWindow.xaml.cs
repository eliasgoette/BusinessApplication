using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplicationProject;
using System.Windows;

namespace BusinessApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AddressViewModel(new Repository<Address>(new AppDbContext()));
        }
    }
}