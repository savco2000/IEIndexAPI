using System.Data.Entity;

namespace DataLayer.Contexts
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            //Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext() : base("name=DefaultConnection") { }
    }
}
