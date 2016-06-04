using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using DataLayer;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.Services
{
    public class AuthorService : IEIndexService<Author>
    {
        public AuthorService(IUnitOfWork uow) : base(uow)
        {
        }

        public override List<Author> GetEntitiesWithChildren(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(x => x.LastName)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .ToList();

    }
}
