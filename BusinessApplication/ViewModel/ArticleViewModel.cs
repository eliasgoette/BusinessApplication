using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using System.ComponentModel;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class ArticleViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly ILogger _logger;

        private List<Article> _searchResults;
        private Article? _selectedArticle;

        private string? _searchArticleNumber;
        private string? _articleNumber;
        private string? _articleName;
        private string? _articlePrice;

        public ArticleViewModel(IRepository<Article> repository, ILogger logger)
        {
            _articleRepository = repository;
            _logger = logger;

            SearchResults = _articleRepository.GetAll().ToList();

            Search = new RelayCommand(ExecuteSearch);
            ClearSelection = new RelayCommand(() => SelectedArticle = null);
            Add = new RelayCommand(async () => await ExecuteAdd());
            Update = new RelayCommand(ExecuteUpdate);
            Remove = new RelayCommand(ExecuteRemove);
        }

        public List<Article> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public Article? SelectedArticle
        {
            get => _selectedArticle;
            set
            {
                _selectedArticle = value;
                OnPropertyChanged(nameof(SelectedArticle));

                ArticleNumber = value?.ArticleNumber;
                ArticleName = value?.Name;
                ArticlePrice = value?.Price.ToString();
            }
        }

        public string? SearchArticleNumber
        {
            get => _searchArticleNumber;
            set
            {
                _searchArticleNumber = value;
                OnPropertyChanged(nameof(SearchArticleNumber));
            }
        }

        public string? ArticleNumber
        {
            get => _articleNumber;
            set
            {
                _articleNumber = value;
                OnPropertyChanged(nameof(ArticleNumber));
            }
        }

        public string? ArticleName
        {
            get => _articleName;
            set
            {
                _articleName = value;
                OnPropertyChanged(nameof(ArticleName));
            }
        }

        public string? ArticlePrice
        {
            get => _articlePrice;
            set
            {
                _articlePrice = value;
                OnPropertyChanged(nameof(ArticlePrice));
            }
        }

        public ICommand Search { get; }
        public ICommand ClearSelection { get; }
        public ICommand Add { get; }
        public ICommand Update { get; }
        public ICommand Remove { get; }

        private void ExecuteSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchArticleNumber))
            {
                SearchResults = _articleRepository.GetAllWhere(x => x.ArticleNumber.Contains(SearchArticleNumber)).ToList();
            }
            else
            {
                SearchResults = _articleRepository.GetAll().ToList();
            }
        }

        private async Task ExecuteAdd()
        {
            if (ValidateInput())
            {
                var newArticle = new Article
                {
                    ArticleNumber = ArticleNumber,
                    Name = ArticleName,
                    Price = double.TryParse(ArticlePrice, out var price) ? price : 0
                };

                await _articleRepository.AddAsync(newArticle);
                ExecuteSearch();
            }
        }

        private void ExecuteUpdate()
        {
            if (ValidateInput())
            {
                if (SelectedArticle != null)
                {
                    SelectedArticle.ArticleNumber = ArticleNumber;
                    SelectedArticle.Name = ArticleName;
                    SelectedArticle.Price = double.TryParse(ArticlePrice, out var price) ? price : 0;

                    _articleRepository.Update(SelectedArticle);
                    ExecuteSearch();
                }
                else
                {
                    _logger.LogMessage("No article selected.");
                }
            }
        }

        private void ExecuteRemove()
        {
            if (SelectedArticle != null)
            {
                _articleRepository.Remove(SelectedArticle);
                ExecuteSearch();
            }
            else
            {
                _logger.LogMessage("No article selected.");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ArticleNumber))
            {
                _logger.LogMessage("Article number can't be undefined.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ArticleName))
            {
                _logger.LogMessage("Article name can't be undefined.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ArticlePrice) || !decimal.TryParse(ArticlePrice, out _))
            {
                _logger.LogMessage("Article price must be a valid number.");
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
