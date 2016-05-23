using System.Data.Entity;
using DataLayer.DomainModels;
using DataLayer.DomainModels.EntityConfigurations;

namespace DataLayer.Contexts
{
    public interface IIEIndexContext : IContext
    {
        IDbSet<Article> Articles { get; }
        IDbSet<Author> Authors { get; }
        IDbSet<Subject> Subjects { get; }
    }

    public class IEIndexContext : BaseContext<IEIndexContext>, IIEIndexContext
    {
        public IDbSet<Article> Articles { get; set; }
        public IDbSet<Author> Authors { get; set; }
        public IDbSet<Subject> Subjects { get; set; }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void SetAdd(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new AuthorConfiguration());
            modelBuilder.Configurations.Add(new SubjectConfiguration());
        }
    }
}
