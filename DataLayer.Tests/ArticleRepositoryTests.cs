using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using Moq;
using Xunit;

namespace DataLayer.Tests
{
    public class when_querying_for_articles : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly Mock<IEIndexContext> _mockContext;

        public when_querying_for_articles(ArticleRepositoryTestsFixture fixture)
        {
            _mockContext = fixture.MockContext;
        }

        [Fact]
        public void all_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;
                var allArticles = sut.All;

                _mockContext.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
            }
        }

        [Fact]
        public void allincluding_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;

                var allArticles = sut.AllIncluding();

                var actualCount = allArticles.Count();

                _mockContext.VerifyAll();

                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void find_should_return_the_correct_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var sut = new ArticleRepository(uow);
                const int expectedId = 2;
                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var article = sut.Find(expectedId);

                _mockContext.VerifyAll();

                Assert.NotNull(article);
                Assert.Equal(expectedId, article.Id);
                Assert.Equal(expectedTitle.ToLowerInvariant(), article.Title.ToLowerInvariant());
            }
        }
    }

    public class when_creating_a_new_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly Mock<IEIndexContext> _mockContext;
        private readonly Article _newArticle;

        public when_creating_a_new_article(ArticleRepositoryTestsFixture fixture)
        {
            _mockContext = fixture.MockContext;
            _newArticle = fixture.NewArticle;
        }

        [Fact]
        public void insertorupdate_should_persist_new_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_newArticle);

                _mockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Once);
                _mockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            }
        }
    }

    public class when_updating_an_existing_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly Mock<IEIndexContext> _mockContext;
        private readonly Article _existingArticle;

        public when_updating_an_existing_article(ArticleRepositoryTestsFixture fixture)
        {
            _mockContext = fixture.MockContext;
            _existingArticle = fixture.ExistingArticle;
        }

        [Fact]
        public void insertorupdate_should_update_existing_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_existingArticle);

                _mockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
                _mockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            }
        }
    }
}
