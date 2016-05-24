using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface IAuthorRepository : IEntityRepository<Author>
    {

    }
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IIEIndexContext _context;

        public AuthorRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = _uow.Context as IIEIndexContext;
        }
        
        public IQueryable<Author> All => _context.Authors;

        public IQueryable<Author> AllIncluding(params Expression<Func<Author, object>>[] includeProperties) => 
            includeProperties.Aggregate(_context.Authors.AsQueryable(), (current, includeProperty) => current.Include(includeProperty));

        public Author Find(int id) => _context.Authors.Find(id);

        public void InsertGraph(Author authorGraph) => _context.Authors.Add(authorGraph);

        public void InsertOrUpdate(Author author)
        {
            if (author.IsNewEntity)
            {
                _context.SetAdd(author);
            }
            else
            {
                _context.SetModified(author);
            }
        }

        public void Delete(Author author) => _context.Authors.Remove(author);

        public void Save() => _uow.Save();
    }
}
