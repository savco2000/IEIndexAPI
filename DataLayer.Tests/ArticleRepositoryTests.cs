using System.Collections.Generic;
using System.Linq;
using DataLayer.DomainModels;
using DataLayer.FakesForTesting;
using DataLayer.Repositories;
using Xunit;

namespace DataLayer.Tests
{
    public class ArticleRepositoryTests
    {
        private readonly List<Article> articles;

        public ArticleRepositoryTests()
        {
            articles = new List<Article>
            {
                new Article
                {
                    Id = 1,
                    Title = "All-in-One Comprehensive Immigration Reform",
                    Page = 4,
                    Issue = Issues.Fall,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
                },
                new Article
                {
                    Id = 2,
                    Title = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa",
                    Page = 20,
                    Issue = Issues.Summer,
                    PublicationYear = PublicationYears.Y2013,
                    IsSupplement = false,
                    Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_voices.pdf"
                }
            };
        }

        [Fact]
        public void all_returns_correct_number_of_articles()
        {
            using (var fakeCtx = new FakeIEIndexContext())
            using (var uow = new UnitOfWork<FakeIEIndexContext>(fakeCtx))
            {
                articles.ForEach(article => fakeCtx.Articles.Add(article));

                var repo = new ArticleRepository(uow);
                const int expectedCount = 2;
                var actualCount = repo.All.Count();

                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void find_returns_correct_article()
        {
            using (var fakeCtx = new FakeIEIndexContext())
            using (var uow = new UnitOfWork<FakeIEIndexContext>(fakeCtx))
            {
                articles.ForEach(article => fakeCtx.Articles.Add(article));

                var repo = new ArticleRepository(uow);
                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var actualTitle = repo.Find(2).Title;

                Assert.Equal(expectedTitle, actualTitle);
            }
        }

        [Fact]
        public void delete_removes_correct_article()
        {
            using (var fakeCtx = new FakeIEIndexContext())
            using (var uow = new UnitOfWork<FakeIEIndexContext>(fakeCtx))
            {
                articles.ForEach(article => fakeCtx.Articles.Add(article));

                var repo = new ArticleRepository(uow);
                const string titleOfArticleToBeDeleted = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";

                var articleToDelete = repo.Find(2);
                repo.Delete(articleToDelete);

                const int expectedCount = 0;
                var actualArticleCount = repo.All.Count(article => article.Title == titleOfArticleToBeDeleted);

                Assert.Equal(expectedCount, actualArticleCount);
            }
        }
    }
}
