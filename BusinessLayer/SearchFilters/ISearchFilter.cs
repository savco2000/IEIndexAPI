using System;
using System.Linq.Expressions;
using DataLayer.DomainModels;

namespace BusinessLayer.SearchFilters
{
    public interface ISearchFilter<TEntity> where TEntity : Entity
    {
        string PageSize { get; set; }
        string PageNumber { get; set; }
        Expression<Func<TEntity, bool>> BuildPredicate();
    }
}
