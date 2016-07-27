using System.Linq;
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
    [Collection("ArticleService Collection")]
    public class when_querying_for_articles
    {
        private readonly ArticleServiceFixture _fixture;

        public when_querying_for_articles(ArticleServiceFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void all_articles_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var mockRepo = new Mock<Repository<Article>>(uow);

                var sut = new ArticleService(mockRepo.Object, new Mock<IMapper>().Object, new Mock<ILog>().Object);

                var expectedCount = _fixture.Articles.Count();
                var allArticles = sut.GetEntities();

                //var allArticlesWithChildren = sut.GetFullEntities();
                
                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allArticles.Count());
                //Assert.Equal(expectedCount, allArticlesWithChildren.Count());
            }
        }
    }
}
