using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public interface ISubjectRepository : IEntityRepository<Subject>
    {

    }
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IUnitOfWork _uow;

        public SubjectRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Subject> All => _uow.Context.Subjects;

        public IQueryable<Subject> AllIncluding(params Expression<Func<Subject, object>>[] includeProperties)
        {
            var query = _uow.Context.Subjects.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public Subject Find(int id) => _uow.Context.Subjects.Find(id);

        public void InsertGraph(Subject subjectGraph) => _uow.Context.Subjects.Add(subjectGraph);

        public void InsertOrUpdate(Subject subject)
        {
            if (subject.IsNewEntity)
            {
                _uow.Context.Entry(subject).State = EntityState.Added;
            }
            else
            {
                _uow.Context.Entry(subject).State = EntityState.Modified;
            }
        }

        public void Delete(Subject subject) => _uow.Context.Subjects.Remove(subject);

        public void Save() => _uow.Save();
    }
}
