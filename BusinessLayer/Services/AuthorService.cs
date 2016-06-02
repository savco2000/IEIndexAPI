using System.Collections.Generic;
using System.Linq;
using BusinessLayer.BindingModels;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public class AuthorService : IEIndexService<Author>
    {
        public AuthorService(Repository<Author> repository) : base(repository)
        {
        }

        public override List<Author> GetEntitiesWithChildren(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(x => x.LastName)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.GetPredicate())
                .ToList();

    }
}
