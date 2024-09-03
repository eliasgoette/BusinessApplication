using Autofac;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using BusinessApplication.ViewModel;
using System.Windows.Controls;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for ArticleView.xaml
    /// </summary>
    public partial class ArticleView : UserControl
    {
        public ArticleView()
        {
            InitializeComponent();
            DataContext = new ArticleViewModel(App.Container.Resolve<IRepository<Article>>(), App.Container.Resolve<ILogger>());
        }
    }
}
