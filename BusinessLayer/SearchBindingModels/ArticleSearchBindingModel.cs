using System;
using System.Linq.Expressions;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.SearchBindingModels
{
    public class ArticleSearchBindingModel : ISearchBindingModel<Article>
    {
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public string Title { get; set; }
        public string Issue { get; set; }
        public string PublicationYear { get; set; }
        public bool? IsSupplement { get; set; }

        public Expression<Func<Article, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.True<Article>();

            if (!string.IsNullOrWhiteSpace(Title))
                predicate = predicate.And(p => p.Title.ToLower() == Title.ToLower());

            var issue = (Issues)Enum.Parse(typeof(Issues), Issue);
            if (!string.IsNullOrWhiteSpace(Issue))
                predicate = predicate.And(p => p.Issue == issue);

            var publicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), PublicationYear);
            if (!string.IsNullOrWhiteSpace(Title))
                predicate = predicate.And(p => p.PublicationYear == publicationYear);

            if (IsSupplement.HasValue)
                predicate = predicate.And(p => p.IsSupplement == IsSupplement);

            return predicate;
        }
    }
}
