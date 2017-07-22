using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer.DomainModels.EntityConfigurations
{
    public class SubjectConfiguration : EntityTypeConfiguration<Subject>
    {
        public SubjectConfiguration()
        {
            ToTable("Subjects");

            HasKey(x => x.Id);

            //Properties
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(200);

            //Relationships
            HasMany(x => x.Articles).WithMany(x => x.Subjects);
        }
    }
}
