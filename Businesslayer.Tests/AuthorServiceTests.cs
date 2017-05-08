using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using AutoMapper;
using BusinessLayer.Services;
using BusinessLayer.Tests.CollectionFixtures;
using Castle.Core.Internal;
using DataLayer;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using log4net;
using Moq;
using Xunit;

namespace BusinessLayer.Tests
{
    [Trait("Category", "AuthorService Unit Tests")]
    [Collection("AuthorService Collection")]
    public class when_querying_for_authors
    {
        private readonly IQueryable<Author> _authors, _authorsWithChildren;
        private readonly Mock<IEIndexContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;

        public when_querying_for_authors(AuthorServiceFixture fixture)
        {
            _authors = fixture.Authors;
            _authorsWithChildren = fixture.AuthorsWithChildren;
            _mockContext = new Mock<IEIndexContext>();
            _mockMapper = fixture.MockMapper;
        }

        [Fact(DisplayName = "All authors should be retrieved minus their children")]
        public void all_authors_should_be_retrieved_minus_their_children()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Author>>(uow);

                mockRepo.SetupGet(x => x.All).Returns(() => _authors);

                var sut = new AuthorService(mockRepo.Object, _mockMapper.Object, new Mock<ILog>().Object);

                var expectedCount = _authors.Count();

                var allAuthors = sut.GetEntities().ToList();

                var noArticleHasChildren = allAuthors.All(x => x.Articles.IsNullOrEmpty());

                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allAuthors.Count);
                Assert.True(noArticleHasChildren);
            }
        }

        [Fact(DisplayName = "All authors should be retrieved along with their children")]
        public void all_authors_should_be_retrieved_along_with_their_children()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Author>>(uow);

                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Author, object>>[]>()))
                    .Returns((Expression<Func<Author, object>>[] includeProperties) => _authorsWithChildren);

                var sut = new AuthorService(mockRepo.Object, _mockMapper.Object, new Mock<ILog>().Object);

                var expectedCount = _authors.Count();

                var allAuthorsWithChildren = sut.GetFullEntities().ToList();

                var atLeastOneAuthorHasChildren = allAuthorsWithChildren.Any(x => !x.Articles.IsNullOrEmpty());

                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allAuthorsWithChildren.Count);
                Assert.True(atLeastOneAuthorHasChildren);
            }
        }
    }

    [Trait("Category", "AuthorService Unit Tests")]
    [Collection("AuthorService Collection")]
    public class when_querying_for_authors_and_database_is_unavailable
    {
        private readonly Mock<IEIndexContext> _mockContext;

        public when_querying_for_authors_and_database_is_unavailable(AuthorServiceFixture fixture)
        {
            _mockContext = new Mock<IEIndexContext>();
        }
        
        [Fact(DisplayName = "Then sql exception should be thrown and logged")]
        public void then_sql_exception_should_be_thrown_and_logged()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Author>>(uow);

                var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

                mockRepo.SetupGet(x => x.All).Throws(exception);
                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Author, object>>[]>())).Throws(exception);

                var mockLog = new Mock<ILog>();
                var sut = new AuthorService(mockRepo.Object, new Mock<IMapper>().Object, mockLog.Object);

                Assert.Throws<SqlException>(() => sut.GetEntities());
                Assert.Throws<SqlException>(() => sut.GetFullEntities());

                mockLog.Verify(log => log.Error(It.IsAny<string>()), Times.Exactly(2));
            }
        }
    }
}
