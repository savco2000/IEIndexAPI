using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using DataLayer.Tests.CollectionFixtures;
using Moq;
using Xunit;

namespace DataLayer.Tests
{
    [Trait("Category", "SubjectRepository Unit Tests")]
    [Collection("SubjectRepository Unit Tests")]
    public class when_querying_for_subjects
    {
        private readonly SubjectRepositoryFixture _fixture;

        public when_querying_for_subjects(SubjectRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void all_authors_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);

                var expectedCount = _fixture.Subjects.Count();
                var allSubjects = sut.All;
                var allSubjectsWithChildren = sut.AllIncluding();

                _fixture.MockContext.VerifyAll();

                Assert.Equal(expectedCount, allSubjects.Count());
                Assert.Equal(expectedCount, allSubjectsWithChildren.Count());
            }
        }

        [Fact]
        public void a_single_author_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);
                const int expectedId = 2;
                const string expectedName = "Global Citizenship";
                var subject = sut.Find(expectedId);
                var actualName = subject.Name;

                _fixture.MockContext.VerifyAll();

                Assert.NotNull(subject);
                Assert.Equal(expectedId, subject.Id);
                Assert.Equal(expectedName.ToLowerInvariant(), actualName.ToLowerInvariant());
            }
        }
    }

    [Trait("Category", "SubjectRepository Unit Tests")]
    [Collection("SubjectRepository Unit Tests")]
    public class when_persisting_a_subject
    {
        private readonly SubjectRepositoryFixture _fixture;

        public when_persisting_a_subject(SubjectRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void if_author_is_new_then_they_should_be_saved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);

                sut.InsertOrUpdate(_fixture.NewSubject);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Subject>()), Times.Once);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Subject>()), Times.Never);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void if_author_is_not_new_then_they_should_be_updated()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);

                sut.InsertOrUpdate(_fixture.ExistingSubject);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Subject>()), Times.Never);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Subject>()), Times.Once);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }

    [Trait("Category", "SubjectRepository Unit Tests")]
    [Collection("SubjectRepository Unit Tests")]
    public class when_deleting_a_subject
    {
        private readonly SubjectRepositoryFixture _fixture;

        public when_deleting_a_subject(SubjectRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact]
        public void if_subject_exists_then_they_should_be_removed()
        {
            var expectedCount = _fixture.Subjects.Count() - 1;
            var articleId = _fixture.Subjects.First().Id;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);

                sut.Delete(articleId);
                sut.Save();

                var actualCount = _fixture.Subjects.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void if_subject_does_not_exist_then_nothing_should_happen()
        {
            var expectedCount = _fixture.Subjects.Count();
            const int nonExistingArticleId = 99999999;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Subject>(uow);

                sut.Delete(nonExistingArticleId);
                sut.Save();

                var actualCount = _fixture.Subjects.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
