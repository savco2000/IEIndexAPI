using DataLayer.Contexts;
using DataLayer.DomainModels;
using Moq;
using Xunit;
using System.Linq;
using DataLayer.Tests.CollectionFixtures;

namespace DataLayer.Tests
{
    [Trait("Category","ArticleRepository Unit Tests")]
    [Collection("ArticleRepository Collection")]
    public class when_querying_for_a_single_article
    {
        private readonly ArticleRepositoryFixture _fixture;

        public when_querying_for_a_single_article(ArticleRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }
        
        [Fact]
        public void article_should_be_retrieved_if_it_exists()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);
                const int expectedArticleId = 2;
                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var article = sut.Find(expectedArticleId);
                
                //_fixture.MockContext.Verify(x => x.Set<Article>(), Times.Once);

                Assert.NotNull(article);
                Assert.Equal(expectedArticleId, article.Id);
                Assert.Equal(expectedTitle.ToLowerInvariant(), article.Title.ToLowerInvariant());
            }
        }

        [Fact]
        public void no_article_should_be_retrieved_if_it_doesnt_exist()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);
                const int nonExistentArticleId = 9999999;
                
                var article = sut.Find(nonExistentArticleId);

                //_fixture.MockContext.Verify(x => x.Set<Article>(), Times.Once);

                Assert.Null(article);
            }
        }
    }

    [Trait("Category", "ArticleRepository Unit Tests")]
    [Collection("ArticleRepository Collection")]
    public class when_querying_for_multiple_articles
    {
        private readonly ArticleRepositoryFixture _fixture;

        public when_querying_for_multiple_articles(ArticleRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void articles_should_be_retrieveable_without_their_children()
        {
            var expectedCount = _fixture.Articles.Count();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                var allArticles = sut.All;

                //_fixture.MockContext.Verify(x => x.Set<Article>(), Times.Exactly(2));

                Assert.Equal(expectedCount, allArticles.Count());
            }
        }

        [Fact]
        public void articles_should_be_retrieveable_with_their_children()
        {
            var expectedCount = _fixture.Articles.Count();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                var allArticlesWithChildren = sut.AllIncluding();

                //_fixture.MockContext.Verify(x => x.Set<Article>(), Times.Exactly(2));

                Assert.Equal(expectedCount, allArticlesWithChildren.Count());
            }
        }
    }

    [Trait("Category", "ArticleRepository Unit Tests")]
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
        public void if_article_is_new_then_it_should_be_persisted()
        {
            var originalCount = _fixture.Articles.Count();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                sut.InsertOrUpdate(_fixture.NewArticle);
                sut.Save();
            }

            _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Once);
            _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);

            var finalCount = _fixture.Articles.Count();

            var expectedCount = originalCount + 1;
            
            Assert.Equal(expectedCount, finalCount);
        }

        [Fact]
        public void if_article_already_exists_then_it_should_be_updated()
        {
            var originalCount = _fixture.Articles.Count();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                sut.InsertOrUpdate(_fixture.ExistingArticle);
                sut.Save();
            }

            _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
            _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
            
            var finalCount = _fixture.Articles.Count();

            var expectedCount = originalCount;

            Assert.Equal(expectedCount, finalCount);
        }

        [Fact]
        public void if_article_is_null_then_nothing_should_happen()
        {
            var originalCount = _fixture.Articles.Count();

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                sut.InsertOrUpdate(null);
                sut.Save();
            }

            _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
            _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
            
            var finalCount = _fixture.Articles.Count();

            var expectedCount = originalCount;

            Assert.Equal(expectedCount, finalCount);
        }
    }

    [Trait("Category", "ArticleRepository Unit Tests")]
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
            var articleId = _fixture.Articles.First().Id;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                sut.Delete(articleId);
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
            const int nonExistingArticleId = 99999999;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new GenericRepository<Article>(uow);

                sut.Delete(nonExistingArticleId);
                sut.Save();

                var actualCount = _fixture.Articles.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
