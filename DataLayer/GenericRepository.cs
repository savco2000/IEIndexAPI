using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer
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

    public class GenericRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private readonly IUnitOfWork _uow;
        private readonly IEIndexContext _context;
        internal DbSet<TEntity> _dbSet;

        public virtual IQueryable<TEntity> All => _dbSet;       

        public GenericRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = uow.Context as IEIndexContext;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = _dbSet.AsQueryable();
            return includeProperties.Aggregate(entities, (current, includeProperty) => current.Include(includeProperty));
        }

        public TEntity Find(int id) => _dbSet.Find(id);

        public void InsertGraph(TEntity entityGraph) => _dbSet.Add(entityGraph);
       
        public void InsertOrUpdate(TEntity entity)
        {
            if(entity == null) return;

            if (entity.IsNewEntity)
                _context.SetAdd(entity);
            else
                _context.SetModified(entity);
        }

        public void Delete(int id)
        {
            var article = _dbSet.Find(id);
            if (article == null) return;
            _context.Set<TEntity>().Remove(article);
        }

        public void Save() => _uow.Save();
    }
}
