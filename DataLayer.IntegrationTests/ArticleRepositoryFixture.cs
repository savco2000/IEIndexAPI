using System;
using System.Data.Entity.Migrations;
using DataLayer.Contexts;
using EntityFramework.Extensions;

namespace DataLayer.IntegrationTests
{
    public class ArticleRepositoryFixture : IDisposable
    {
        public ArticleRepositoryFixture()
        {
            MigrateDbToLatestVersion();
        }

        private static void MigrateDbToLatestVersion()
        {
            var configuration = new Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        public void Dispose()
        {
            var context = new IEIndexContext();
            context.Articles.Delete();
            context.Authors.Delete();
            context.Subjects.Delete();
        }
    }
}
