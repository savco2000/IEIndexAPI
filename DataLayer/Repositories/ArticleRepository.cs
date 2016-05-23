using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.Repositories;

namespace UnitOfWorkTest.Repositories
{
    public interface IArticleRepository : IEntityRepository<Article>
    {

    }
    public class ArticleRepository : IArticleRepository
    {
        private readonly IUnitOfWork _uow;

        public ArticleRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Article> All => _uow.Context.Articles;

        public IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties)
        {
            var query = _uow.Context.Articles.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            
            return query;
        }

        public Article Find(int id) => _uow.Context.Articles.Find(id);

        public void InsertGraph(Article articleGraph) => _uow.Context.Articles.Add(articleGraph);
       
        public void InsertOrUpdate(Article article)
        {
            if (article.IsNewEntity)
            {
                _uow.Context.Entry(article).State = EntityState.Added;
            }
            else  
            {
                _uow.Context.Entry(article).State = EntityState.Modified;
            }
        }

        public void Delete(Article article) => _uow.Context.Articles.Remove(article);
       
        public void Save() => _uow.Save();
    }
}