using Xunit;

namespace DataLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("ArticleRepository Collection")]
    public class ArticleRepositoryCollection : ICollectionFixture<ArticleRepositoryFixture>
    {
    }
}
