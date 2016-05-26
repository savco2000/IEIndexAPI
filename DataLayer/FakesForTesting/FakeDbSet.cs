using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.DomainModels;

namespace DataLayer.FakesForTesting
{
    public class FakeDbSet<TEntity> : IDbSet<TEntity> where TEntity : Entity, new()
    {
        private readonly ObservableCollection<TEntity> _items;
        private readonly IQueryable _query;

        public FakeDbSet()
        {
            _items = new ObservableCollection<TEntity>();
            _query = _items.AsQueryable();
        }
        public TEntity Add(TEntity entity)
        {
            _items.Add(entity);
            return entity;
        }
        public TEntity Attach(TEntity entity)
        {
            _items.Add(entity);
            return entity;
        }
        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity 
            => Activator.CreateInstance<TDerivedEntity>();
       
        public TEntity Create() => new TEntity();

        public TEntity Find(params object[] keyValues)
        {
            var keyValue = keyValues.FirstOrDefault();
            return keyValue != null ? this.SingleOrDefault(entity => entity.Id == (int)keyValue) : null;
        }

        public ObservableCollection<TEntity> Local => _items;

        public TEntity Remove(TEntity entity)
        {
            _items.Remove(entity);
            return entity;
        }
        public IEnumerator<TEntity> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public Type ElementType => _query.ElementType;

        public Expression Expression => _query.Expression;

        public IQueryProvider Provider => _query.Provider;
    }
}
