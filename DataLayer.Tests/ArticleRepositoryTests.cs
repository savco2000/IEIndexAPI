using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using Moq;
using Xunit;
using System.Linq;

namespace DataLayer.Tests
{
    [Trait("Category","Articles")]
    public class when_querying_for_articles : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly ArticleRepositoryTestsFixture _fixture;

        public when_querying_for_articles(ArticleRepositoryTestsFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void all_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;
                var allArticles = sut.All;

                _fixture.MockContext.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
            }
        }

        [Fact]
        public void allincluding_should_return_all_the_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;

                var allArticles = sut.AllIncluding();

                var actualCount = allArticles.Count();

                _fixture.MockContext.VerifyAll();

                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void find_should_return_the_correct_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);
                const int expectedId = 2;
                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var article = sut.Find(expectedId);

                _fixture.MockContext.VerifyAll();

                Assert.NotNull(article);
                Assert.Equal(expectedId, article.Id);
                Assert.Equal(expectedTitle.ToLowerInvariant(), article.Title.ToLowerInvariant());
            }
        }
    }

    [Trait("Category", "Articles")]
    public class when_creating_a_new_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly ArticleRepositoryTestsFixture _fixture;

        public when_creating_a_new_article(ArticleRepositoryTestsFixture fixture)
        {
            fixture.MockContext.ResetCalls();

            _fixture = fixture;
        }

        [Fact]
        public void insertorupdate_should_persist_new_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_fixture.NewArticle);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Once);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void insertorupdate_should_update_existing_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_fixture.ExistingArticle);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }

    [Trait("Category", "Articles")]
    public class when_updating_an_existing_article : IClassFixture<ArticleRepositoryTestsFixture>
    {
        private readonly ArticleRepositoryTestsFixture _fixture;

        public when_updating_an_existing_article(ArticleRepositoryTestsFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void insertorupdate_should_update_existing_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(_fixture.ExistingArticle);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
