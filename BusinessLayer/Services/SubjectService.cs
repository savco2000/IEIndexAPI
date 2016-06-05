using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using DataLayer;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.Services
{
    public class SubjectService : IEIndexService<Subject>
    {
        public SubjectService(IUnitOfWork uow) : base(uow)
        {
        }

        public override List<Subject> GetEntitiesWithChildren(ISearchBindingModel<Subject> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(x => x.Articles)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .ToList();
    }
}
