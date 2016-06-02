using System.Collections.Generic;
using System.Linq;
using BusinessLayer.BindingModels;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public class SubjectService : IEIndexService<Subject>
    {
        public SubjectService(Repository<Subject> repository) : base(repository)
        {
        }

        public override List<Subject> GetEntitiesWithChildren(ISearchBindingModel<Subject> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(x => x.Articles)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.GetPredicate())
                .ToList();
    }
}
