using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;
using Moq;

namespace BusinessLayer.Tests.CollectionFixtures
{
    public class ArticleServiceFixture
    {
        public Mock<IMapper> MockMapper;
        public Article NewArticle { get; set; }
        public Article ExistingArticle { get; set; }
        public IQueryable<Article> Articles { get; set; }
        public IQueryable<Article> ArticlesWithChildren { get; set; }

        public ArticleServiceFixture()
        {
            MockMapper = CreateMockMapper();

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

            Articles = CreateArticles().AsQueryable();
            ArticlesWithChildren = CreateArticlesWithChildren().AsQueryable();
        }

        private static Mock<IMapper> CreateMockMapper()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<ArticleVM>(It.IsAny<Article>()))
                .Returns((Article source) =>
                {
                    var target = new ArticleVM
                    {
                        Title = source.Title,
                        Page = source.Page,
                        Issue = source.Issue.GetEnumDescription(),
                        PublicationYear = source.PublicationYear.GetEnumDescription(),
                        IsSupplement = source.IsSupplement,
                        Hyperlink = source.Hyperlink,
                        Authors = source.Authors.Select(author => new AuthorVM
                        {
                            FirstName = author.FirstName,
                            LastName = author.LastName,
                            Suffix = author.Suffix.GetEnumDescription()
                        }),
                        Subjects = source.Subjects.Select(x => new SubjectVM {Name = x.Name})
                    };

                    return target;
                });

            mockMapper.Setup(x => x.Map<Article>(It.IsAny<ArticleVM>()))
               .Returns((ArticleVM source) =>
               {
                   var target = new Article
                   {
                       Title = source.Title,
                       Page = source.Page,
                       Issue = (Issues)Enum.Parse(typeof(Issues), source.Issue),
                       PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), source.PublicationYear),
                       IsSupplement = source.IsSupplement,
                       Hyperlink = source.Hyperlink,
                       Authors = source.Authors.Select(author => new Author
                       {
                           FirstName = author.FirstName,
                           LastName = author.LastName,
                           Suffix = (Suffixes)Enum.Parse(typeof(Suffixes), author.Suffix)
                       }) as ICollection<Author>,
                       Subjects = source.Subjects.Select(subject => new Subject { Name = subject.Name }) as ICollection<Subject>
                   };

                   return target;
               });

            mockMapper.Setup(x => x.Map<AuthorVM>(It.IsAny<Author>()))
                .Returns((Author source) =>
                {
                    var target = new AuthorVM
                    {
                        FirstName = source.FirstName,
                        LastName = source.LastName,
                        Suffix = source.Suffix.GetEnumDescription(),
                        Articles = source.Articles.Select(article => new ArticleVM
                        {
                            Title = article.Title,
                            Page = article.Page,
                            Issue = article.Issue.GetEnumDescription(),
                            PublicationYear = article.PublicationYear.GetEnumDescription(),
                            IsSupplement = article.IsSupplement,
                            Hyperlink = article.Hyperlink
                        })
                    };

                    return target;
                });

            mockMapper.Setup(x => x.Map<Author>(It.IsAny<AuthorVM>()))
                .Returns((AuthorVM source) =>
                {
                    var target = new Author
                    {
                        FirstName = source.FirstName,
                        LastName = source.LastName,
                        Suffix = (Suffixes)Enum.Parse(typeof(Suffixes), source.Suffix),
                        Articles = source.Articles.Select(article => new Article
                        {
                            Title = article.Title,
                            Page = article.Page,
                            Issue = (Issues)Enum.Parse(typeof(Issues), article.Issue),
                            PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), article.PublicationYear),
                            IsSupplement = article.IsSupplement,
                            Hyperlink = article.Hyperlink
                        }) as ICollection<Article>
                    };

                    return target;
                });

            mockMapper.Setup(x => x.Map<SubjectVM>(It.IsAny<Subject>()))
                .Returns((Subject source) =>
                {
                    var target = new SubjectVM
                    {
                        Name = source.Name,
                        Articles = source.Articles.Select(article => new ArticleVM
                        {
                            Title = article.Title,
                            Page = article.Page,
                            Issue = article.Issue.GetEnumDescription(),
                            PublicationYear = article.PublicationYear.GetEnumDescription(),
                            IsSupplement = article.IsSupplement,
                            Hyperlink = article.Hyperlink
                        })
                    };

                    return target;
                });

            mockMapper.Setup(x => x.Map<Subject>(It.IsAny<SubjectVM>()))
                .Returns((SubjectVM source) =>
                {
                    var target = new Subject
                    {
                        Name = source.Name,
                        Articles = source.Articles.Select(article => new Article
                        {
                            Title = article.Title,
                            Page = article.Page,
                            Issue = (Issues)Enum.Parse(typeof(Issues), article.Issue),
                            PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), article.PublicationYear),
                            IsSupplement = article.IsSupplement,
                            Hyperlink = article.Hyperlink
                        }) as ICollection<Article>
                    };

                    return target;
                });

            return mockMapper;
        }

        private static IEnumerable<Article> CreateArticles()
        {
            var articles = new List<Article> {
                new Article {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
                },
                new Article {
                    Id = 2,
                    Title = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa",
                    Page = 20,
                    Issue = Issues.Summer,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_voices.pdf"
                },
                new Article {
                    Id = 3,
                    Title = "A Champion for Development, Security, and Human Rights: An Interview with Kofi Annan",
                    Page = 4,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_mayjun13_voices.pdf"
                },
                new Article {
                    Id = 4,
                    Title = "A Perspective on Communism's Collapse and European Higher Education",
                    Page = 34,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false
                },
                new Article {
                    Id = 5,
                    Title = "Building a Literate World",
                    Page = 14,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false
                }
            };

            return articles;
        }

        private static IEnumerable<Article> CreateArticlesWithChildren()
        {
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

            return articles;
        }
    }
}
