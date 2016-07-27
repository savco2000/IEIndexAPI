using Xunit;

namespace BusinessLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("ArticleService Collection")]
    public class ArticleServiceCollection : ICollectionFixture<ArticleServiceFixture>
    {
    }
}
