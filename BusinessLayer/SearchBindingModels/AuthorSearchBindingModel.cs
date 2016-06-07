using System;
using System.Linq.Expressions;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.SearchBindingModels
{
    public class AuthorSearchBindingModel : ISearchBindingModel<Author>
    {
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        
        public Expression<Func<Author, bool>> SearchFilter()
        {
            var predicate = PredicateBuilder.True<Author>();

            if (!string.IsNullOrWhiteSpace(FirstName))
                predicate = predicate.And(p => p.FirstName.ToLower() == FirstName.ToLower());

            if (!string.IsNullOrWhiteSpace(LastName))
                predicate = predicate.And(p => p.LastName.ToLower() == LastName.ToLower());

            Suffixes suffix;
            if (Enum.TryParse(Suffix, true, out suffix))
                predicate = predicate.And(p => p.Suffix == suffix);

            return predicate;
        }
    }
}
