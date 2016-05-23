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
    public class ArticleFakeDbSet : FakeDbSet<Article>
    {
        public override Article Find(params object[] keyValues)
        {
            var keyValue = keyValues.FirstOrDefault();
            return keyValue != null ? this.SingleOrDefault(article => article.Id == (int)keyValue) : null;
        }
    }

    public class AuthorFakeDbSet : FakeDbSet<Author>
    {
        public override Author Find(params object[] keyValues)
        {
            var keyValue = keyValues.FirstOrDefault();
            return keyValue != null ? this.SingleOrDefault(author => author.Id == (int)keyValue) : null;
        }
    }

    public class SubjectFakeDbSet : FakeDbSet<Subject>
    {
        public override Subject Find(params object[] keyValues)
        {
            var keyValue = keyValues.FirstOrDefault();
            return keyValue != null ? this.SingleOrDefault(subject => subject.Id == (int)keyValue) : null;
        }
    }

    #region Abstract FakeDbSet

    public abstract class FakeDbSet<TEntity> : IDbSet<TEntity> where TEntity : Entity, new()
    {
        readonly ObservableCollection<TEntity> _items;
        readonly IQueryable _query;

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
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
        public TEntity Create()
        {
            return new TEntity();
        }
        public abstract TEntity Find(params object[] keyValues);

        public ObservableCollection<TEntity> Local => _items;

        public TEntity Remove(TEntity entity)
        {
            _items.Remove(entity);
            return entity;
        }
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        public Type ElementType => _query.ElementType;

        public Expression Expression => _query.Expression;

        public IQueryProvider Provider => _query.Provider;
    }

    #endregion
}
