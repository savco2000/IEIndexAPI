using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface IArticleRepository : IEntityRepository<Article>
    {

    }
    public class ArticleRepository : IArticleRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IIEIndexContext _context;

        public ArticleRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = uow.Context as IIEIndexContext;
        }

        public IQueryable<Article> All => _context.Articles;

        public IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties)
        {
            var query = _context.Articles.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            
            return query;
        }

        public Article Find(int id) => _context.Articles.Find(id);

        public void InsertGraph(Article articleGraph) => _context.Articles.Add(articleGraph);
       
        public void InsertOrUpdate(Article article)
        {
            if (article.IsNewEntity)
            {
                _uow.Context.SetAdd(article);
            }
            else
            {
                _uow.Context.SetModified(article);
            }
        }

        public void Delete(Article article) => _context.Articles.Remove(article);
       
        public void Save() => _uow.Save();
    }
}