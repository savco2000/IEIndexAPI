using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer.DomainModels.EntityConfigurations
{
    public class AuthorConfiguration : EntityTypeConfiguration<Author>
    {
        public AuthorConfiguration()
        {
            ToTable("Authors");

            HasKey(x => x.Id);

            //Properties
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            Property(x => x.LastName).IsRequired().HasMaxLength(50);
            Property(x => x.Suffix).IsOptional();

            //Relationships
            HasMany(x => x.Articles);
        }
    }
}
