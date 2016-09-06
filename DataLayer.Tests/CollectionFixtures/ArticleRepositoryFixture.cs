using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using Moq;

namespace DataLayer.Tests.CollectionFixtures
{
    public class ArticleRepositoryFixture
    {
        public Mock<IEIndexContext> MockContext { get; set; }
        public Article NewArticle { get; set; }
        public Article ExistingArticle { get; set; }
        public IQueryable<Article> Articles { get; set; }

        public ArticleRepositoryFixture()
        {
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

            Articles = new List<Article> {
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
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(Articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(Articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(Articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(Articles.GetEnumerator());

            mockSet.Setup(x => x.Find(It.IsAny<object[]>()))
                .Returns((object[] keyValues) =>
                {
                    var keyValue = keyValues.FirstOrDefault();
                    return keyValue != null ? Articles.SingleOrDefault(article => article.Id == (int)keyValue) : null;
                });

            mockSet.Setup(x => x.Remove(It.IsAny<Article>()))
                .Returns((Article articleToDelete) =>
                {
                    Articles = Articles.Where(article => article.Id != articleToDelete.Id);
                    return articleToDelete;
                });

            MockContext = new Mock<IEIndexContext>();
            MockContext.Setup(m => m.Set<Article>()).Returns(mockSet.Object);

            MockContext.Setup(x => x.SetAdd(It.IsAny<object>())).Callback<object>(entity =>
            {
                var articles = Articles.ToList();
                articles.Add((Article) entity);
                Articles = articles.AsQueryable();
            });

            MockContext.Setup(x => x.SetModified(It.IsAny<object>())).Callback<object>(entity =>
            {
                var updatedArticle = (Article) entity;

                var articles = Articles.ToList();
                var originalArticle = articles.Single(article => article.Id == updatedArticle.Id);

                articles.Remove(originalArticle);
                articles.Add(updatedArticle);

                Articles = articles.AsQueryable();
            });
        }
    }
}
