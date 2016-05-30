using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using Moq;
using Xunit;
using System.Linq;

namespace DataLayer.Tests
{
    [Trait("Category","ArticleRepository")]
    [Collection("ArticleRepository Collection")]
    public class when_querying_for_articles
    {
        private readonly ArticleRepositoryFixture _fixture;

        public when_querying_for_articles(ArticleRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void all_articles_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                var expectedCount = _fixture.Articles.Count();
                var allArticles = sut.All;
                var allArticlesWithChildren = sut.AllIncluding();

                _fixture.MockContext.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
                Assert.Equal(expectedCount, allArticlesWithChildren.Count());
            }
        }

        [Fact]
        public void a_single_article_should_be_retrieved()
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

    [Trait("Category", "ArticleRepository")]
    [Collection("ArticleRepository Collection")]
    public class when_persisting_an_article
    {
        private readonly ArticleRepositoryFixture _fixture;

        public when_persisting_an_article(ArticleRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();

            _fixture = fixture;
        }

        [Fact]
        public void if_article_is_new_then_it_should_be_saved()
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
        public void if_article_is_not_new_then_it_should_be_updated()
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

    [Trait("Category", "ArticleRepository")]
    [Collection("ArticleRepository Collection")]
    public class when_deleting_an_article
    {
        private readonly ArticleRepositoryFixture _fixture;

        public when_deleting_an_article(ArticleRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();

            _fixture = fixture;
        }

        [Fact]
        public void if_article_exists_then_it_should_be_removed()
        {
            var expectedCount = _fixture.Articles.Count() - 1;
            var article = _fixture.Articles.FirstOrDefault();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.Delete(article);
                sut.Save();
                
                var actualCount = _fixture.Articles.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void if_article_does_not_exist_then_nothing_should_happen()
        {
            var expectedCount = _fixture.Articles.Count();
            var nonExistingArticle = new Article
            {
                Title = "Hello World!",
                Page = 16,
                Issue = Issues.Mar_Apr,
                PublicationYear = PublicationYears.Y2013,
                IsSupplement = true
            };

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new ArticleRepository(uow);

                sut.Delete(nonExistingArticle);
                sut.Save();

                var actualCount = _fixture.Articles.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
