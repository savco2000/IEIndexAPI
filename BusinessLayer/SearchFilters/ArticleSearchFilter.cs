using System;
using System.Linq.Expressions;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.SearchFilters
{
    public class ArticleSearchFilter : ISearchFilter<Article>
    {
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public string Title { get; set; }
        public string Issue { get; set; }
        public string PublicationYear { get; set; }
        public bool? IsSupplement { get; set; }

        public Expression<Func<Article, bool>> Filter()
        {
            var predicate = PredicateBuilder.New<Article>();

            if (!string.IsNullOrWhiteSpace(Title))
                predicate = predicate.And(p => p.Title.ToLower() == Title.ToLower());

            if (Enum.TryParse(Issue, true, out Issues issue))
                predicate = predicate.And(p => p.Issue == issue);

            if (Enum.TryParse(PublicationYear, true, out PublicationYears publicationYear))
                predicate = predicate.And(p => p.PublicationYear == publicationYear);

            if (IsSupplement.HasValue)
                predicate = predicate.And(p => p.IsSupplement == IsSupplement);

            return predicate;
        }
    }
}
