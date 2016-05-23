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

    public class IEIndexContext : BaseContext<IEIndexContext>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new AuthorConfiguration());
            modelBuilder.Configurations.Add(new SubjectConfiguration());
        }
    }
}
