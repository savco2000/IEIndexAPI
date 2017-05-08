using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Tests.CollectionFixtures;
using Moq;
using Xunit;

namespace DataLayer.Tests
{
    [Trait("Category", "AuthorRepository Unit Tests")]
    [Collection("ArticleRepository Unit Tests")]
    public class when_querying_for_authors
    {
        private readonly AuthorRepositoryFixture _fixture;

        public when_querying_for_authors(AuthorRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact(DisplayName = "All authors should be retrieved")]
        public void all_authors_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);

                var expectedCount = _fixture.Authors.Count();
                var allAuthors = sut.All;
                var allAuthorsWithChildren = sut.AllIncluding();

                _fixture.MockContext.VerifyAll();

                Assert.Equal(expectedCount, allAuthors.Count());
                Assert.Equal(expectedCount, allAuthorsWithChildren.Count());
            }
        }

        [Fact(DisplayName = "A single author should be retrieved")]
        public void a_single_author_should_be_retrieved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);
                const int expectedId = 2;
                const string expectedFullName = "Elaina Loveland";
                var author = sut.Find(expectedId);
                var actualFullName = $"{author.FirstName} {author.LastName}";

                _fixture.MockContext.VerifyAll();

                Assert.NotNull(author);
                Assert.Equal(expectedId, author.Id);
                Assert.Equal(expectedFullName.ToLowerInvariant(), actualFullName.ToLowerInvariant());
            }
        }
    }

    [Trait("Category", "AuthorRepository Unit Tests")]
    [Collection("ArticleRepository Unit Tests")]
    public class when_persisting_an_author
    {
        private readonly AuthorRepositoryFixture _fixture;

        public when_persisting_an_author(AuthorRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }

        [Fact(DisplayName = "If author is new then they should be saved")]
        public void if_author_is_new_then_they_should_be_saved()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);

                sut.InsertOrUpdate(_fixture.NewAuthor);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Author>()), Times.Once);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Author>()), Times.Never);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact(DisplayName = "If author is not new then they should be updated")]
        public void if_author_is_not_new_then_they_should_be_updated()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);

                sut.InsertOrUpdate(_fixture.ExistingAuthor);
                sut.Save();

                _fixture.MockContext.Verify(x => x.SetAdd(It.IsAny<Author>()), Times.Never);
                _fixture.MockContext.Verify(x => x.SetModified(It.IsAny<Author>()), Times.Once);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }

    [Trait("Category", "AuthorRepository Unit Tests")]
    [Collection("ArticleRepository Unit Tests")]
    public class when_deleting_an_author
    {
        private readonly AuthorRepositoryFixture _fixture;

        public when_deleting_an_author(AuthorRepositoryFixture fixture)
        {
            fixture.MockContext.ResetCalls();
            _fixture = fixture;
        }
        
        [Fact(DisplayName = "If author exists then they should be removed")]
        public void if_author_exists_then_they_should_be_removed()
        {
            var expectedCount = _fixture.Authors.Count() - 1;
            var articleId = _fixture.Authors.First().Id;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);

                sut.Delete(articleId);
                sut.Save();

                var actualCount = _fixture.Authors.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact(DisplayName = "If article does not exist then nothing should happen")]
        public void if_article_does_not_exist_then_nothing_should_happen()
        {
            var expectedCount = _fixture.Authors.Count();
            const int nonExistingArticleId = 99999999;

            using (var uow = new UnitOfWork<IEIndexContext>(_fixture.MockContext.Object))
            {
                var sut = new Repository<Author>(uow);

                sut.Delete(nonExistingArticleId);
                sut.Save();

                var actualCount = _fixture.Authors.Count();

                Assert.Equal(expectedCount, actualCount);
            }

            _fixture.MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
