using System;
using System.Data.Entity;
using DataLayer.Contexts;
using DataLayer.DomainModels;

namespace DataLayer.FakesForTesting
{
    public class FakeIEIndexContext : IIEIndexContext
    {
        public IDbSet<Article> Articles { get; }
        public IDbSet<Author> Authors { get; }
        public IDbSet<Subject> Subjects { get; }

        public FakeIEIndexContext()
        {
            Articles = new ArticleFakeDbSet();
            Authors = new AuthorFakeDbSet();
            Subjects = new SubjectFakeDbSet();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void SetModified(object entity)
        {
            throw new NotImplementedException();
        }

        public void SetAdd(object entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}
