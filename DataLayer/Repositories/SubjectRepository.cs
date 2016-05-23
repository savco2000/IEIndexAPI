using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface ISubjectRepository : IEntityRepository<Subject>
    {

    }
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IIEIndexContext _context;

        public SubjectRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = uow.Context as IIEIndexContext;
        }

        public IQueryable<Subject> All => _context.Subjects;

        public IQueryable<Subject> AllIncluding(params Expression<Func<Subject, object>>[] includeProperties)
        {
            var query = _context.Subjects.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public Subject Find(int id) => _context.Subjects.Find(id);

        public void InsertGraph(Subject subjectGraph) => _context.Subjects.Add(subjectGraph);

        public void InsertOrUpdate(Subject subject)
        {
            if (subject.IsNewEntity)
            {
                _uow.Context.SetAdd(subject);
            }
            else
            {
                _uow.Context.SetModified(subject);
            }
        }

        public void Delete(Subject subject) => _context.Subjects.Remove(subject);

        public void Save() => _uow.Save();
    }
}
