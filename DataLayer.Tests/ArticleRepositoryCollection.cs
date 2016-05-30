using Xunit;

namespace DataLayer.Tests
{
    [CollectionDefinition("ArticleRepository Collection")]
    public class ArticleRepositoryCollection : ICollectionFixture<ArticleRepositoryFixture>
    {
    }
}
