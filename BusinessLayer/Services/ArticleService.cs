using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public class ArticleService : IEIndexService<Article>
    {
        public ArticleService(Repository<Article> repository) : base(repository)
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
