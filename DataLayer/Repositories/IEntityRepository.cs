using System;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> All { get; }
        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Find(int id);
        void InsertGraph(TEntity entity);
        void InsertOrUpdate(TEntity entity);
        void Delete(int id);
        void Save();
    }
}
