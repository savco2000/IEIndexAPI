using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer.DbConfigurations
{
    internal class IEIndexDBInitializer : CreateDatabaseIfNotExists<IEIndexContext>
    {
        protected override void Seed(IEIndexContext context)
        {
            var authors = new List<Author>
            {
                new Author { FirstName = "Stuart", LastName = "Anderson" },
                new Author { FirstName = "Elaina", LastName = "Loveland" },
                new Author { FirstName = "Susan", LastName = "Ladika" }
            };

            authors.ForEach(author => context.Authors.AddOrUpdate(x => new { x.FirstName, x.LastName, x.Suffix }, author));
            context.SaveChanges();

            var subjects = new List<Subject>
            {
                new Subject {Name = "Ethics" },
                new Subject {Name= "Global Citizenship" },
                new Subject {Name = "Higher Education" },
                new Subject {Name = "Public Policy" }
            };

            subjects.ForEach(subject => context.Subjects.AddOrUpdate(x => x.Name, subject));
            context.SaveChanges();

            var articles = new List<Article> {
                new Article {
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

            articles.ForEach(article => context.Articles.AddOrUpdate(x => x.Title, article));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
