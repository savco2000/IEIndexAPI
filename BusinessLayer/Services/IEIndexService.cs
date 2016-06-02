using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public abstract class IEIndexService<TEntity> where TEntity : Entity
    {
        protected readonly Repository<TEntity> Repository;

        protected IEIndexService(Repository<TEntity> repository)
        {
            Repository = repository;
        }

        public List<TEntity> GetEntities(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber) => Repository.All
                       .OrderBy(x => x.Id)
                       .Skip(pageNumber * pageSize - pageSize)
                       .Take(pageSize)
                       .AsExpandable()
                       .Where(searchParameters.GetPredicate())
                       .ToList();
       
        public abstract List<TEntity> GetEntitiesWithChildren(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber);

        public TEntity Find(int id) => Repository.Find(id);

        public void InsertGraph(TEntity articleGraph) => Repository.InsertGraph(articleGraph);

        public void InsertOrUpdate(TEntity article) => Repository.InsertOrUpdate(article);

        public void Delete(int id) => Repository.Delete(id);
    }
}
