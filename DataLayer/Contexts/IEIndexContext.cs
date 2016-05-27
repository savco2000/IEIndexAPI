using System.Data.Entity;
using DataLayer.DomainModels;
using DataLayer.DomainModels.EntityConfigurations;

namespace DataLayer.Contexts
{
    public interface IIEIndexContext : IContext
    {
        DbSet<Article> Articles { get; }
        DbSet<Author> Authors { get; }
        DbSet<Subject> Subjects { get; }
    }

    public class IEIndexContext : BaseContext<IEIndexContext>, IIEIndexContext
    {
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetAdd(object entity)
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
