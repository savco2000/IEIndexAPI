using System.Linq;
using DataLayer.Contexts;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using Xunit;

namespace DataLayer.IntegrationTests
{

    [Trait("Category", "ArticleRepository Integration Tests")]
    [Collection("MyCollection")]
    public class when_querying_for_articles
    {
        [Fact]
        public void all_articles_should_be_retrieved()
        {
            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);

                var countIsLargerThanOne = sut.All.Count() > 1;

                Assert.True(countIsLargerThanOne);
            }
        }

        [Fact]
        public void all_articles_should_be_retrieved_along_with_their_children()
        {
            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);
               
                var allArticlesWithChildren = sut.AllIncluding();

                var atleastOneArticleHasChildren = allArticlesWithChildren.Any(x => x.Authors.Any() || x.Subjects.Any());
                var countIsLargerThanOne = allArticlesWithChildren.Count() > 1;

                Assert.True(countIsLargerThanOne);
                Assert.True(atleastOneArticleHasChildren);
            }
        }

        [Fact]
        public void a_single_article_should_be_retrieved()
        {
            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);
                const int expectedId = 2;
                const string expectedTitle = "From Undocumented Immigrant to Brain Surgeon: An Interview with Alfredo Quiñones-Hinojosa";
                var article = sut.Find(expectedId);

                Assert.NotNull(article);
                Assert.Equal(expectedId, article.Id);
                Assert.Equal(expectedTitle.ToLowerInvariant(), article.Title.ToLowerInvariant());
            }
        }
    }

    [Trait("Category", "ArticleRepository Integration Tests")]
    [Collection("MyCollection")]
    public class when_persisting_an_article
    {
        [Fact]
        public void if_article_is_new_then_it_should_be_saved()
        {
            int expectedCount, actualCount;

            var newArticle = new Article
            {
                Title = "Hello World!",
                Page = 16,
                Issue = Issues.Mar_Apr,
                PublicationYear = PublicationYears.Y2013,
                IsSupplement = true
            };

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);

                expectedCount = sut.All.Count() + 1;

                sut.InsertOrUpdate(newArticle);
                sut.Save();
            }

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                actualCount = new ArticleRepository(uow).All.Count();
            }

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void if_article_is_not_new_then_it_should_be_updated()
        {
            int expectedCount, actualCount;

            var existingArticle = new Article
            {
                Id = 1,
                Title = "All-in-One Comprehensive Immigration Reform",
                Page = 44,
                Issue = Issues.Winter,
                PublicationYear = PublicationYears.Y2016,   
                IsSupplement = true,
                Hyperlink = "http://www.nafsa.org/_/File/_/ie_julaug13_frontlines.pdf"
            };

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);

                expectedCount = sut.All.Count();

                sut.InsertOrUpdate(existingArticle);
                sut.Save();
            }

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                actualCount = new ArticleRepository(uow).All.Count();
            }

            Assert.Equal(expectedCount, actualCount);
        }
    }

    [Trait("Category", "ArticleRepository Integration Tests")]
    [Collection("MyCollection")]
    public class when_deleting_an_article
    {
        [Fact]
        public void if_article_exists_then_it_should_be_removed()
        {
            int expectedCount, actualCount;

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                var sut = new ArticleRepository(uow);

                expectedCount = sut.All.Count() - 1;

                sut.Delete(4);
                sut.Save();
            }

            using (var context = new IEIndexContext())
            using (var uow = new UnitOfWork<IEIndexContext>(context))
            {
                actualCount = new ArticleRepository(uow).All.Count();
            }

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
