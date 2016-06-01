using DataLayer.IntegrationTests;
using Xunit;

namespace DataLayer.Tests
{
    [CollectionDefinition("ArticleRepository Integration Tests Collection")]
    public class ArticleRepositoryCollection : ICollectionFixture<ArticleRepositoryFixture>
    {
    }
}
