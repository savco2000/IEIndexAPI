using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using DataLayer;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.Services
{
    public class ArticleService : IEIndexService<Article>
    {
        public ArticleService(IUnitOfWork uow) : base(uow)
        {
            
        }

        public override List<Article> GetEntitiesWithChildren(ISearchBindingModel<Article> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Authors, x => x.Subjects)
                .OrderBy(x => x.Title)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.GetPredicate())
                .ToList();
    }
}
