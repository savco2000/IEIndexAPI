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
        private readonly IEIndexContext _context;

        public SubjectRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = uow.Context as IEIndexContext;
        }

        public IQueryable<Subject> All => _context.Subjects;

        public IQueryable<Subject> AllIncluding(params Expression<Func<Subject, object>>[] includeProperties) => 
            includeProperties.Aggregate(_context.Subjects.AsQueryable(), (current, includeProperty) => current.Include(includeProperty));

        public Subject Find(int id) => _context.Subjects.Find(id);

        public void InsertGraph(Subject subjectGraph) => _context.Subjects.Add(subjectGraph);

        public void InsertOrUpdate(Subject subject)
        {
            if (subject.IsNewEntity)
                _context.SetAdd(subject);
            else
                _context.SetModified(subject);
        }

        public void Delete(Subject subject) => _context.Subjects.Remove(subject);

        public void Save() => _uow.Save();
    }
}
