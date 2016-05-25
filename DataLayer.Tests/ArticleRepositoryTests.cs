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
        private readonly Mock<IIEIndexContext> _mockIEIndexContext;

        public when_querying_for_articles(ArticleRepositoryTestsFixture fixture)
        {
            _mockIEIndexContext = fixture.MockIEIndexContext;
        }

        [Fact]
        public void all_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockIEIndexContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;
                var actualCount = sut.All.Count();
                
                _mockIEIndexContext.VerifyAll();

                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void allincluding_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockIEIndexContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;

                var actualCount = sut.AllIncluding(x => x.Authors, x => x.Subjects).Count();

                _mockIEIndexContext.VerifyAll();

                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void find_should_return_the_correct_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockIEIndexContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var actualTitle = sut.Find(2).Title;

                _mockIEIndexContext.VerifyAll();

                Assert.Equal(expectedTitle.ToLowerInvariant(), actualTitle.ToLowerInvariant());
            }
        }
    }

    public class when_creating_a_new_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly Mock<IIEIndexContext> _mockIEIndexContext;
        private readonly Article _newArticle;

        public when_creating_a_new_article(ArticleRepositoryTestsFixture fixture)
        {
            _mockIEIndexContext = fixture.MockIEIndexContext;
            _newArticle = fixture.NewArticle;
        }

        [Fact]
        public void insertorupdate_should_persist_new_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockIEIndexContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_newArticle);

                _mockIEIndexContext.VerifyAll();

                _mockIEIndexContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Once);
                _mockIEIndexContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            }
        }
    }

    public class when_updating_an_existing_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly Mock<IIEIndexContext> _mockIEIndexContext;
        private readonly Article _existingArticle;

        public when_updating_an_existing_article(ArticleRepositoryTestsFixture fixture)
        {
            _mockIEIndexContext = fixture.MockIEIndexContext;
            _existingArticle = fixture.ExistingArticle;
        }

        [Fact]
        public void insertorupdate_should_update_existing_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockIEIndexContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_existingArticle);

                _mockIEIndexContext.VerifyAll();

                _mockIEIndexContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
                _mockIEIndexContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            }
        }
    }
}
