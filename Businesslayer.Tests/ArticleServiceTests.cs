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

                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Article, object>>[]>()))
                    .Returns((Expression<Func<Article, object>>[] includeProperties) => _articlesWithChildren);

                mockRepo.SetupGet(x => x.All).Returns(() => _articles);

                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, new Mock<ILog>().Object);

                var expectedCount = _articles.Count();
                var allArticles = sut.GetEntities();

                var allArticlesWithChildren = sut.GetFullEntities();
                
                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
                Assert.Equal(expectedCount, allArticlesWithChildren.Count());
            }
        }

        [Fact]
        public void if_database_is_unavailable_then_sql_exception_should_be_thrown_and_logged_()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Article>>(uow);

                var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

                mockRepo.SetupGet(x => x.All).Throws(exception);

                var mockLog = new Mock<ILog>();
                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, mockLog.Object);

                Assert.Throws<SqlException>(() => sut.GetEntities());

                mockLog.Verify(x => x.Error(It.IsAny<string>()), Times.Once);
            }
        }
    }
}
