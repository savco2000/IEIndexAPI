using DataLayer.DomainModels;

namespace DataLayer.Repositories
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(IUnitOfWork uow) 
            : base(uow)
        {
            
        }
    }
}