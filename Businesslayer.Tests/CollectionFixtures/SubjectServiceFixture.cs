﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLayer.DTOs;
using DataLayer.DomainModels;
using Moq;

namespace BusinessLayer.Tests.CollectionFixtures
{
    public class SubjectServiceFixture
    {
        public Mock<IMapper> MockMapper;
        public Subject NewSubject { get; set; }
        public Subject ExistingSubject { get; set; }
        public IQueryable<Subject> Subjects { get; set; }
        public IQueryable<Subject> SubjectsWithChildren { get; set; }

        public SubjectServiceFixture()
        {
            MockMapper = CreateMockMapper();

            NewSubject = new Subject { Name = "The Ottomans" };

            ExistingSubject = new Subject { Id = 1, Name = "Ethics" };

            Subjects = CreateSubjects().AsQueryable();
            SubjectsWithChildren = CreateSubjectsWithChildren().AsQueryable();
        }

        private static Mock<IMapper> CreateMockMapper()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<SubjectDTO>(It.IsAny<Subject>()))
                .Returns((Subject source) =>
                {
                    var target = new SubjectDTO
                    {
                        Name = source.Name,
                        Articles = source.Articles.Select(article => article.Title)
                    };

                    return target;
                });

            return mockMapper;
        }

        private static IEnumerable<Subject> CreateSubjects()
        {
            var subjects = new List<Subject>
            {
                new Subject
                {
                    Id = 1,
                    Name = "Ethics"
                },
                new Subject
                {
                    Id = 2,
                    Name = "Global Citizenship"
                },
                new Subject
                {
                    Id = 3,
                    Name = "Higher Education"
                },
                new Subject
                {
                    Id = 4,
                    Name = "Public Policy"
                }
            };

            return subjects;
        }

        private static IEnumerable<Subject> CreateSubjectsWithChildren()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Stuart", LastName = "Anderson" },
                new Author { Id = 2,FirstName = "Elaina", LastName = "Loveland" },
                new Author { Id = 3,FirstName = "Susan", LastName = "Ladika" }
            };

            var articles = new List<Article>
            {
                new Article
                {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf",
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Stuart" && x.LastName == "Anderson")}
                },
                new Article
                {
                    Id = 2,
                    Title = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa",
                    Page = 20,
                    Issue = Issues.Summer,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_voices.pdf",
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland")}
                },
                new Article
                {
                    Id = 3,
                    Title = "A Champion for Development, Security, and Human Rights: An Interview with Kofi Annan",
                    Page = 4,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_mayjun13_voices.pdf",
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland")}
                },
                new Article
                {
                    Id = 4,
                    Title = "A Perspective on Communism's Collapse and European Higher Education",
                    Page = 34,
                    Issue = Issues.Spring,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Elaina" && x.LastName == "Loveland")}
                },
                new Article
                {
                    Id = 5,
                    Title = "Building a Literate World",
                    Page = 14,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Authors = new List<Author> {authors.Single(x => x.FirstName == "Susan" && x.LastName == "Ladika")}
                }
            };

            var subjects = new List<Subject>
            {
                new Subject
                {
                    Id = 1,
                    Name = "Ethics",
                    Articles = new List<Article> {articles.Single(x => x.Title == "All-in-One Comprehensive Immigration Reform")}
                },
                new Subject
                {
                    Id = 2,
                    Name = "Global Citizenship",
                    Articles = new List<Article> {articles.Single(x => x.Title == "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa")}
                },
                new Subject
                {
                    Id = 3,
                    Name = "Higher Education",
                    Articles = new List<Article> {articles.Single(x => x.Title == "A Champion for Development, Security, and Human Rights: An Interview with Kofi Annan")}
                },
                new Subject
                {
                    Id = 4,
                    Name = "Public Policy",
                    Articles = new List<Article> {articles.Single(x => x.Title == "A Perspective on Communism's Collapse and European Higher Education")}
                }
            };

            return subjects;
        }
    }
}
