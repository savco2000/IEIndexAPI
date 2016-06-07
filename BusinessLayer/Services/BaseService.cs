using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLayer.SearchBindingModels;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public abstract class BaseService<TEntityVM, TEntity> where TEntity : Entity where TEntityVM : class, new()
    {
        protected readonly Repository<TEntity> Repository;
        protected readonly IMapper Mapper;
        private readonly IMapper _mapper;

        protected BaseService(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            Repository = new Repository<TEntity>(uow);
        }

        public List<TEntityVM> GetEntities(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber) => Repository.All
                   .OrderBy(entity => entity.Id)
                   .Skip(pageNumber * pageSize - pageSize)
                   .Take(pageSize)
                   .AsExpandable()
                   .Where(searchParameters.SearchFilter())
                   .ToList()
                   .Select(entity => _mapper.Map<TEntityVM>(entity))
                   .ToList();

        public abstract List<TEntityVM> GetEntitiesWithChildren(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber);

        public TEntityVM Find(int id) => AutoMapper.Mapper.Map<TEntityVM>(Repository.Find(id));

        public void InsertGraph(TEntityVM entityVMGraph) => Repository.InsertGraph(AutoMapper.Mapper.Map<TEntity>(entityVMGraph));
        
        public void InsertOrUpdate(TEntityVM entityVM) => Repository.InsertOrUpdate(AutoMapper.Mapper.Map<TEntity>(entityVM));

        public void Delete(int id) => Repository.Delete(id);
    }
}
