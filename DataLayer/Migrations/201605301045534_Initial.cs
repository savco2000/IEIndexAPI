namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Page = c.Int(nullable: false),
                        Issue = c.Int(nullable: false),
                        PublicationYear = c.Int(nullable: false),
                        IsSupplement = c.Boolean(nullable: false),
                        Hyperlink = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Suffix = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleAuthors",
                c => new
                    {
                        Article_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.Author_Id })
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Article_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.SubjectArticles",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Article_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Article_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubjectArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.SubjectArticles", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.ArticleAuthors", "Author_Id", "dbo.Authors");
            DropForeignKey("dbo.ArticleAuthors", "Article_Id", "dbo.Articles");
            DropIndex("dbo.SubjectArticles", new[] { "Article_Id" });
            DropIndex("dbo.SubjectArticles", new[] { "Subject_Id" });
            DropIndex("dbo.ArticleAuthors", new[] { "Author_Id" });
            DropIndex("dbo.ArticleAuthors", new[] { "Article_Id" });
            DropTable("dbo.SubjectArticles");
            DropTable("dbo.ArticleAuthors");
            DropTable("dbo.Subjects");
            DropTable("dbo.Authors");
            DropTable("dbo.Articles");
        }
    }
}
