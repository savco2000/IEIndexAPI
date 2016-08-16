using Xunit;

namespace BusinessLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("AuthorService Collection")]
    public class AuthorServiceCollection : ICollectionFixture<AuthorServiceFixture>
    {
    }
}
