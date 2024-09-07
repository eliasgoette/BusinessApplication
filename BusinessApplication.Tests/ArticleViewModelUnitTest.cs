using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using BusinessApplication.ViewModel;
using Moq;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class ArticleViewModelUnitTest
    {
        private Mock<IRepository<Article>> _mockArticleRepository;
        private Mock<ILogger> _mockLogger;
        private ArticleViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange:
            _mockArticleRepository = new Mock<IRepository<Article>>();
            _mockLogger = new Mock<ILogger>();

            _viewModel = new ArticleViewModel(_mockArticleRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void ArticleViewModel_InitializesCorrectly()
        {
            // Arrange
            var articles = new List<Article> {
                new Article {
                    ArticleNumber = "AR123",
                    Name = "Article 1",
                    Price = 10.99
                }
            };
            _mockArticleRepository.Setup(repo => repo.GetAll()).Returns(articles.AsQueryable());

            // Act
            _viewModel = new ArticleViewModel(_mockArticleRepository.Object, _mockLogger.Object);

            // Assert
            Assert.IsNotNull(_viewModel.SearchResults);
            Assert.AreEqual(1, _viewModel.SearchResults.Count);
            Assert.AreEqual("AR123", _viewModel.SearchResults[0].ArticleNumber);
        }

        [TestMethod]
        public void ExecuteSearch_SearchesCorrectly()
        {
            // Arrange
            var articles = new List<Article>
            {
                new Article { ArticleNumber = "AR123", Name = "Article 1", Price = 10.99 },
                new Article { ArticleNumber = "AR456", Name = "Article 2", Price = 20.99 }
            };
            _mockArticleRepository.Setup(repo => repo.GetAllWhere(It.IsAny<System.Linq.Expressions.Expression<System.Func<Article, bool>>>()))
                                  .Returns((System.Linq.Expressions.Expression<System.Func<Article, bool>> predicate) =>
                                      articles.AsQueryable().Where(predicate));

            // Act
            _viewModel.SearchArticleNumber = "AR123";
            _viewModel.Search.Execute(this);

            // Assert
            Assert.AreEqual(1, _viewModel.SearchResults.Count);
            Assert.AreEqual("AR123", _viewModel.SearchResults[0].ArticleNumber);
        }

        [TestMethod]
        public void ExecuteAdd_AddsArticleCorrectly()
        {
            // Arrange
            _viewModel.ArticleNumber = "AR123";
            _viewModel.ArticleName = "New Article";
            _viewModel.ArticlePrice = "15.99";

            // Act
            _viewModel.Add.Execute(this);

            // Assert
            _mockArticleRepository.Verify(repo => repo.AddAsync(It.Is<Article>(a =>
                a.ArticleNumber == "AR123" &&
                a.Name == "New Article" &&
                a.Price == 15.99
            )), Times.Once);

            _mockArticleRepository.Verify(repo => repo.AddAsync(It.IsAny<Article>()), Times.Once);
        }

        [TestMethod]
        public void ExecuteUpdate_UpdatesArticleCorrectly()
        {
            // Arrange
            var article = new Article
            {
                ArticleNumber = "AR123",
                Name = "Article 1",
                Price = 10.99
            };

            _viewModel.SelectedArticle = article;
            _viewModel.ArticleNumber = "AR123";
            _viewModel.ArticleName = "Updated Article";
            _viewModel.ArticlePrice = "19.99";

            // Act
            _viewModel.Update.Execute(this);

            // Assert
            _mockArticleRepository.Verify(repo => repo.Update(It.Is<Article>(a => a == article)), Times.Once);
            _mockArticleRepository.Verify(repo => repo.Update(It.IsAny<Article>()), Times.Once);
        }

        [TestMethod]
        public void ExecuteRemove_RemovesArticleCorrectly()
        {
            // Arrange
            var article = new Article
            {
                ArticleNumber = "AR123",
                Name = "Article 1",
                Price = 10.99
            };

            _viewModel.SelectedArticle = article;

            // Act
            _viewModel.Remove.Execute(this);

            // Assert
            _mockArticleRepository.Verify(repo => repo.Remove(It.Is<Article>(a => a.ArticleNumber == "AR123")), Times.Once);
        }
    }
}