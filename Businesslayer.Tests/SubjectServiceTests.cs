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
    [Trait("Category", "SubjectService Unit Tests")]
    [Collection("SubjectService Collection")]
    public class when_querying_for_subjects
    {
        private readonly IQueryable<Subject> _subjects, _subjectsWithChildren;
        private readonly Mock<IEIndexContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;

        public when_querying_for_subjects(SubjectServiceFixture fixture)
        {
            _subjects = fixture.Subjects;
            _subjectsWithChildren = fixture.SubjectsWithChildren;
            _mockContext = new Mock<IEIndexContext>();
            _mockMapper = fixture.MockMapper;
        }
        
        [Fact(DisplayName = "All subjects should be retrieved minus their children")]
        public void all_subjects_should_be_retrieved_minus_their_children()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Subject>>(uow);

                mockRepo.SetupGet(x => x.All).Returns(() => _subjects);

                var sut = new SubjectService(mockRepo.Object, _mockMapper.Object, new Mock<ILog>().Object);

                var expectedCount = _subjects.Count();

                var allSubjects = sut.GetEntities().ToList();

                var noSubjectHasChildren = allSubjects.All(x => x.Articles.IsNullOrEmpty());

                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allSubjects.Count);
                Assert.True(noSubjectHasChildren);
            }
        }

        [Fact(DisplayName = "All subjects should be retrieved along with their children")]
        public void all_subjects_should_be_retrieved_along_with_their_children()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Subject>>(uow);

                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Subject, object>>[]>()))
                    .Returns((Expression<Func<Subject, object>>[] includeProperties) => _subjectsWithChildren);

                var sut = new SubjectService(mockRepo.Object, _mockMapper.Object, new Mock<ILog>().Object);

                var expectedCount = _subjects.Count();

                var allSubjectsWithChildren = sut.GetFullEntities().ToList();

                var atLeastOneArticleHasChildren = allSubjectsWithChildren.Any(x => !x.Articles.IsNullOrEmpty());

                mockRepo.VerifyAll();

                Assert.Equal(expectedCount, allSubjectsWithChildren.Count);
                Assert.True(atLeastOneArticleHasChildren);
            }
        }
    }

    [Trait("Category", "SubjectService Unit Tests")]
    [Collection("SubjectService Collection")]
    public class when_querying_for_subjects_and_database_is_unavailable
    {
        private readonly Mock<IEIndexContext> _mockContext;

        public when_querying_for_subjects_and_database_is_unavailable(SubjectServiceFixture fixture)
        {
            _mockContext = new Mock<IEIndexContext>();
        }
       
        [Fact(DisplayName = "Then sql exception should be thrown and logged")]
        public void then_sql_exception_should_be_thrown_and_logged()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_mockContext.Object))
            {
                var mockRepo = new Mock<Repository<Subject>>(uow);

                var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

                mockRepo.SetupGet(x => x.All).Throws(exception);
                mockRepo.Setup(x => x.AllIncluding(It.IsAny<Expression<Func<Subject, object>>[]>())).Throws(exception);

                var mockLog = new Mock<ILog>();
                var sut = new SubjectService(mockRepo.Object, new Mock<IMapper>().Object, mockLog.Object);

                Assert.Throws<SqlException>(() => sut.GetEntities());
                Assert.Throws<SqlException>(() => sut.GetFullEntities());

                mockLog.Verify(log => log.Error(It.IsAny<string>()), Times.Exactly(2));
            }
        }
    }
}
