using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer.DomainModels.EntityConfigurations
{
    public class ArticleConfiguration : EntityTypeConfiguration<Article>
    {
        public ArticleConfiguration()
        {
            ToTable("Articles");

            HasKey(x => x.Id);

            //Properties
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).IsRequired().HasMaxLength(200);
            Property(x => x.Page).IsRequired();
            Property(x => x.Issue).IsRequired();
            Property(x => x.PublicationYear).IsRequired();
            Property(x => x.IsSupplement).IsRequired();
            Property(x => x.Hyperlink).IsOptional().HasMaxLength(200);

            //Relationships
            HasMany(x => x.Authors).WithMany(x => x.Articles);
        }
    }
}
