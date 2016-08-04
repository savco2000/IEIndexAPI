using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using AutoMapper;
using BusinessLayer.Services;
using BusinessLayer.Tests.CollectionFixtures;
using DataLayer;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using log4net;
using Moq;
using Xunit;

namespace BusinessLayer.Tests
{
    [Trait("Category", "ArticleService Unit Tests")]
    [Collection("ArticleService Collection")]
    public class when_querying_for_articles
    {
        private readonly IQueryable<Article> _articles, _articlesWithChildren;
        private readonly Mock<IEIndexContext> _mockContext;

        public when_querying_for_articles(ArticleServiceFixture fixture)
        {
            _articles = fixture.Articles;
            _articlesWithChildren = fixture.ArticlesWithChildren;
            _mockContext = new Mock<IEIndexContext>();
        }

        [Fact]
        public void all_articles_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Article>>(uow);

                mockRepo.SetupGet(x => x.All).Returns(() => _articles);

                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, new Mock<ILog>().Object);

                var expectedCount = _articles.Count();
                var allArticles = sut.GetEntities();
                
                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
            }
        }

        [Fact]
        public void all_articles_should_be_retrieved_along_with_their_children()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Article>>(uow);

                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Article, object>>[]>()))
                    .Returns((Expression<Func<Article, object>>[] includeProperties) => _articlesWithChildren);

                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, new Mock<ILog>().Object);

                var expectedCount = _articles.Count();

                var allArticlesWithChildren = sut.GetFullEntities().ToList();

                var atLeastOneArticleHasChildren = allArticlesWithChildren.Any(x => x.Authors != null && x.Subjects != null);

                mockRepo.VerifyAll();
                
                Assert.Equal(expectedCount, allArticlesWithChildren.Count);
                Assert.True(atLeastOneArticleHasChildren);
            }
        }
    }

    [Trait("Category", "ArticleService Unit Tests")]
    [Collection("ArticleService Collection")]
    public class when_querying_for_articles_and_database_is_unavailable
    {
        private readonly Mock<IEIndexContext> _mockContext;

        public when_querying_for_articles_and_database_is_unavailable(ArticleServiceFixture fixture)
        {
            _mockContext = new Mock<IEIndexContext>();
        }

        [Fact]
        public void then_sql_exception_should_be_thrown_and_logged()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Article>>(uow);

                var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

                mockRepo.SetupGet(x => x.All).Throws(exception);
                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Article, object>>[]>())).Throws(exception);

                var mockLog = new Mock<ILog>();
                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, mockLog.Object);

                Assert.Throws<SqlException>(() => sut.GetEntities());
                Assert.Throws<SqlException>(() => sut.GetFullEntities());

                mockLog.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(2));
            }
        }
    }
}
