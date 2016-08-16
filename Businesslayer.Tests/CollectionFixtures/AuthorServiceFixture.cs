using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLayer.DTOs;
using DataLayer.DomainModels;
using Moq;

namespace BusinessLayer.Tests.CollectionFixtures
{
    public class AuthorServiceFixture
    {
        public Mock<IMapper> MockMapper;
        public Author NewAuthor { get; set; }
        public Author ExistingAuthor { get; set; }
        public IQueryable<Author> Authors { get; set; }
        public IQueryable<Author> AuthorsWithChildren { get; set; }

        public AuthorServiceFixture()
        {
            MockMapper = CreateMockMapper();

            NewAuthor = new Author { FirstName = "Jon", LastName = "Snow" };

            ExistingAuthor = new Author { Id = 1, FirstName = "Stuart", LastName = "Anderson", Suffix = Suffixes.Jr };

            Authors = CreateAuthors().AsQueryable();

            AuthorsWithChildren = CreateAuthorsWithChildren().AsQueryable();
        }

        private static Mock<IMapper> CreateMockMapper()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<AuthorDTO>(It.IsAny<Author>()))
                .Returns((Author source) =>
                {
                    var target = new AuthorDTO
                    {
                        FullName = source.FullName,
                        Articles = source.Articles.Select(article => article.Title)
                    };

                    return target;
                });

            return mockMapper;
        }

        private static IEnumerable<Author> CreateAuthors()
        {
            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "Stuart",
                    LastName = "Anderson"
                },
                new Author
                {
                    Id = 2,
                    FirstName = "Elaina",
                    LastName = "Loveland"
                },
                new Author
                {
                    Id = 3,
                    FirstName = "Susan",
                    LastName = "Ladika"
                }
            };

            return authors;
        }

        private static IEnumerable<Author> CreateAuthorsWithChildren()
        {
            var subjects = new List<Subject>
            {
                new Subject {Id = 1,Name = "Ethics" },
                new Subject {Id = 2,Name= "Global Citizenship" },
                new Subject {Id = 3,Name = "Higher Education" },
                new Subject {Id = 4,Name = "Public Policy" }
            };

            var articles = new List<Article>
            {
                new Article {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf",
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
                    Subjects = new List<Subject>
                    {
                        subjects.Single(x => x.Name == "Higher Education"),
                        subjects.Single(x => x.Name == "Global Citizenship")
                    }
                }
            };

            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "Stuart",
                    LastName = "Anderson",
                    Articles = new List<Article>
                    {
                        articles.Single(x => x.Title == "All-in-One Comprehensive Immigration Reform")
                    }
                },
                new Author
                {
                    Id = 2,
                    FirstName = "Elaina",
                    LastName = "Loveland",
                    Articles = new List<Article>
                    {
                        articles.Single(x => x.Title == "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa"),
                        articles.Single(x => x.Title == "A Champion for Development, Security, and Human Rights: An Interview with Kofi Annan")
                    }
                },
                new Author
                {
                    Id = 3,
                    FirstName = "Susan",
                    LastName = "Ladika",
                    Articles = new List<Article>
                    {
                        articles.Single(x => x.Title == "Building a Literate World")
                    }
                }
            };

            return authors;
        }
    }
}
