using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface IAuthorRepository : IEntityRepository<Author>
    {

    }
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IUnitOfWork _uow;

        public AuthorRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Author> All => _uow.Context.Authors;

        public IQueryable<Author> AllIncluding(params Expression<Func<Author, object>>[] includeProperties)
        {
            var query = _uow.Context.Authors.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public Author Find(int id) => _uow.Context.Authors.Find(id);

        public void InsertGraph(Author authorGraph) => _uow.Context.Authors.Add(authorGraph);

        public void InsertOrUpdate(Author author)
        {
            if (author.IsNewEntity)
            {
                _uow.Context.Entry(author).State = EntityState.Added;
            }
            else
            {
                _uow.Context.Entry(author).State = EntityState.Modified;
            }
        }

        public void Delete(Author author) => _uow.Context.Authors.Remove(author);

        public void Save() => _uow.Save();
    }
}
