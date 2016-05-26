using System.Collections.Generic;
using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.FakesForTesting;
using Moq;

namespace DataLayer.Tests
{
    public class ArticleRepositoryTestsFixture
    {
        public Mock<IIEIndexContext> MockIEIndexContext { get; set; }
        public Article NewArticle { get; set; }
        public Article ExistingArticle { get; set; }

        public ArticleRepositoryTestsFixture()
        {
            MockIEIndexContext = new Mock<IIEIndexContext>();
            MockIEIndexContext.Setup(x => x.Articles).Returns(new FakeDbSet<Article>());

            NewArticle = new Article
            {
                Title = "All-in-One Comprehensive Immigration Reform",
                Page = 4,
                Issue = Issues.Fall,
                PublicationYear = PublicationYears.Y2013,
                IsSupplement = false,
                Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
            };

            ExistingArticle = new Article
            {
                Id = 1,
                Title = "All-in-One Comprehensive Immigration Reform",
                Page = 4,
                Issue = Issues.Fall,
                PublicationYear = PublicationYears.Y2013,
                IsSupplement = false,
                Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
            };

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

            articles.ForEach(article => MockIEIndexContext.Object.Articles.Add(article));
        }
    }
}
