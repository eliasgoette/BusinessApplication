using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.ViewModel;
using BusinessApplicationProject;
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
            var logger = new Logger();
            logger.AddLoggingService(new PopupLoggingService());
            var customerRepository = new Repository<Article>(() => new AppDbContext(), App.AppLogger);
            DataContext = new ArticleViewModel(articleRepository, App.AppLogger);
        }
    }
}
