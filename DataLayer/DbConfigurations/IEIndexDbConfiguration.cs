using System.Data.Entity;

namespace DataLayer.DbConfigurations
{
    internal class IEIndexDbConfiguration : DbConfiguration
    {
        public IEIndexDbConfiguration()
        {
            SetDatabaseInitializer(new IEIndexDBInitializer());
        }
    }
}
