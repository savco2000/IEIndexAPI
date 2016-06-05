using Xunit;

namespace DataLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("SubjectRepository Unit Tests")]
    public class SubjectRepositoryCollection : ICollectionFixture<SubjectRepositoryFixture>
    {
    }
}
