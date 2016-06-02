using System;
using System.Linq.Expressions;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.BindingModels
{
    public class SubjectSearchBindingModel : ISearchBindingModel<Subject>
    {
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public string Name { get; set; }
        public Expression<Func<Subject, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.True<Subject>();

            if (!string.IsNullOrWhiteSpace(Name))
                predicate = predicate.And(p => p.Name.ToLower() == Name.ToLower());

            return predicate;
        }
    }
}
