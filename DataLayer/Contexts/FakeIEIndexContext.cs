using System;
using System.Data.Entity;
using DataLayer.DomainModels;

namespace DataLayer.Contexts
{
    public class FakeIEIndexContext : IIEIndexContext
    {
        public IDbSet<Article> Articles { get; }
        public IDbSet<Author> Authors { get; }
        public IDbSet<Subject> Subjects { get; }
        public void SetModified(object entity)
        {
            throw new NotImplementedException();
        }

        public void SetAdd(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
