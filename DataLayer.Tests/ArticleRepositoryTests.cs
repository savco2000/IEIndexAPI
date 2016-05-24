using System.Collections.Generic;
using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.FakesForTesting;
using DataLayer.Repositories;
using Moq;
using Xunit;

namespace DataLayer.Tests
{
    public class ArticleRepositoryTests
    {
        private readonly Mock<IIEIndexContext> _mockIEIndexContext;
        private readonly IIEIndexContext _fakeCtx;

        public ArticleRepositoryTests()
        {
            _mockIEIndexContext = new Mock<IIEIndexContext>();
            _mockIEIndexContext.Setup(x => x.Articles).Returns(new ArticleFakeDbSet());

            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Stuart", LastName = "Anderson" },
                new Author { Id = 2,FirstName = "Elaina", LastName = "Loveland" },
                new Author { Id = 3,FirstName = "Susan", LastName = "Ladika" }
            };

            var subjects = new List<Subject>
            {
                new Subject {Id = 1,Name = "Ethics" },
                new Subject {Id = 2,Name= "Global Citizenship" },
                new Subject {Id = 3,Name = "Higher Education" },
                new Subject {Id = 4,Name = "Public Policy" }
            };

            var articles = new List<Article> {
                new Article {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf",
                    Authors = new List<Author> { authors.Single(x => x.FirstName == "Stuart" && x.LastName == "Anderson") },
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Higher Education"),
                        subjects.Single(x => x.Name == "Public Policy")
                    }
                },
                new Article {
                    Id = 2,
                    Title = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa",
                    Page = 20,
                    Issue = Issues.Summer,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_voices.pdf",
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland") },
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Higher Education"),
                        subjects.Single(x => x.Name == "Public Policy"),
                        subjects.Single(x => x.Name == "Global Citizenship")
                    }
                },
                new Article {
                    Id = 3,
                    Title = "A Champion for Development, Security, and Human Rights: An Interview with Kofi Annan",
                    Page = 4,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_mayjun13_voices.pdf",
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland") },
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Ethics"),
                        subjects.Single(x => x.Name == "Global Citizenship")
                    }
                },
                new Article {
                    Id = 4,
                    Title = "A Perspective on Communism's Collapse and European Higher Education",
                    Page = 34,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland") },
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Higher Education"),
                        subjects.Single(x => x.Name == "Public Policy")
                    }
                },
                new Article {
                    Id = 5,
                    Title = "Building a Literate World",
                    Page = 14,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Susan" && x.LastName == "Ladika") },
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Higher Education"),
                        subjects.Single(x => x.Name == "Global Citizenship")
                    }
                }
            };

            _fakeCtx = _mockIEIndexContext.Object;
            articles.ForEach(article => _fakeCtx.Articles.Add(article));
        }

        [Fact]
        public void all_returns_correct_number_of_articles()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fakeCtx))
            {
                var sut = new ArticleRepository(uow);

                const int expectedCount = 5;
                var actualCount1 = sut.All.Count();
                var actualCount2 = sut.AllIncluding(x => x.Authors).Count();

                _mockIEIndexContext.VerifyGet(x => x.Articles);

                Assert.Equal(expectedCount, actualCount1);
                Assert.Equal(expectedCount, actualCount2);
            }
        }

        [Fact]
        public void find_returns_correct_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fakeCtx))
            {
                var sut = new ArticleRepository(uow);

                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var actualTitle = sut.Find(2).Title;

                _mockIEIndexContext.VerifyGet(x => x.Articles);

                Assert.Equal(expectedTitle, actualTitle);
            }
        }

        [Fact]
        public void delete_removes_correct_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fakeCtx))
            {
                var sut = new ArticleRepository(uow);

                var articleToDelete = sut.Find(2);
                var titleOfArticleToBeDeleted = articleToDelete.Title;
                sut.Delete(articleToDelete);

                const int expectedCount = 0;
                var actualArticleCount = sut.All.Count(article => article.Title == titleOfArticleToBeDeleted);

                _mockIEIndexContext.VerifyGet(x => x.Articles);
               
                Assert.Equal(expectedCount, actualArticleCount);
            }
        }

        [Fact]
        public void insertorupdate_inserts_new_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fakeCtx))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(new Article
                {
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
                });

                _mockIEIndexContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Once);
                _mockIEIndexContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Never);
            }
        }

        [Fact]
        public void insertorupdate_updates_existing_article()
        {
            using (var uow = new UnitOfWork<IEIndexContext>(_fakeCtx))
            {
                var sut = new ArticleRepository(uow);

                sut.InsertOrUpdate(new Article
                {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
                });

                _mockIEIndexContext.Verify(x => x.SetAdd(It.IsAny<Article>()), Times.Never);
                _mockIEIndexContext.Verify(x => x.SetModified(It.IsAny<Article>()), Times.Once);
            }
        }
    }
}
