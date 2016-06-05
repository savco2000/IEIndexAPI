using Xunit;

namespace DataLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("ArticleRepository Unit Tests")]
    public class AuthorRepositoryCollection : ICollectionFixture<AuthorRepositoryFixture>
    {
    }
}
