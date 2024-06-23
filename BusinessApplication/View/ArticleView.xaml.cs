using BusinessApplication.Model;
using BusinessApplication.Repository;
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
            var articleRepository = new Repository<Article>(() => new AppDbContext(), App.AppLogger);
            DataContext = new ArticleViewModel(articleRepository);
        }
    }
}
