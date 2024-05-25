using BusinessApplication.Model;
using BusinessApplication.Repository;

namespace BusinessApplication.ViewModel
{
    public class ArticleViewModel
    {
        private readonly IRepository<Article> _articleRepository;

        public ArticleViewModel(IRepository<Article> repository)
        {
            _articleRepository = repository;
        }
    }
}
