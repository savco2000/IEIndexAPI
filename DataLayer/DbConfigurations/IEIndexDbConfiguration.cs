using System.Data.Entity;
using DataLayer.Contexts;

namespace DataLayer.DbConfigurations
{
    internal class IEIndexDbConfiguration : DbConfiguration
    {
        public IEIndexDbConfiguration()
        {
            SetDatabaseInitializer(new NullDatabaseInitializer<IEIndexContext>());
        }
    }
}
