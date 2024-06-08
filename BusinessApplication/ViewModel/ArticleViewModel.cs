using BusinessApplication.Model;
using BusinessApplication.Repository;
using System.ComponentModel;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class ArticleViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<Article> _articleRepository;

        private List<Article> _searchResults;
        private Article? _selectedArticle;

        private string? _searchArticleNumber;
        private string? _articleNumber;
        private string? _articleName;
        private string? _articlePrice;

        public ArticleViewModel(IRepository<Article> repository)
        {
            _articleRepository = repository;
            _searchResults = _articleRepository.GetAll().ToList();

            Search = new RelayCommand(() =>
            {
                if (SearchArticleNumber != null)
                {
                    _searchResults = _articleRepository.GetAllWhere(x => x.ArticleNumber == SearchArticleNumber).ToList();
                }
                else
                {
                    _searchResults = _articleRepository.GetAll().ToList();
                }
            });
        }
        public List<Article> SearchResults => _searchResults;
        public Article? SelectedArticle
        {
            get => _selectedArticle;
            set
            {
                _selectedArticle = value;
                OnPropertyChanged(nameof(SelectedArticle));

            }
        }
        public string? SearchArticleNumber
        {
            get { return _searchArticleNumber; }
            set { _searchArticleNumber = value; 
                OnPropertyChanged(nameof(SearchArticleNumber));}
        }
        public string? ArticleNumber
        {
            get { return _articleNumber; }
            set { _articleNumber = value;
                OnPropertyChanged(nameof(ArticleNumber));}
        }
        public string? ArticleName
        {
            get { return _articleName; }
            set{ _articleName = value;
                OnPropertyChanged(nameof(ArticleName));}
        }
        public string? ArticlePrice
        {
            get { return _articlePrice; }
            set { _articlePrice = value;
            OnPropertyChanged(nameof(ArticlePrice));}
        }
        public ICommand Search { get; }
        public ICommand ClearSelection { get; }
        public ICommand Add { get; }
        public ICommand Update { get; }
        public ICommand Remove { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
