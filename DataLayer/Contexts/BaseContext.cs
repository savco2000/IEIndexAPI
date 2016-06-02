using System.Data.Entity;

namespace DataLayer.Contexts
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        protected BaseContext() : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
