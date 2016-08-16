using Xunit;

namespace BusinessLayer.Tests.CollectionFixtures
{
    [CollectionDefinition("SubjectService Collection")]
    public class SubjectServiceCollection : ICollectionFixture<SubjectServiceFixture>
    {
    }
}
