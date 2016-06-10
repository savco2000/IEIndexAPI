using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using BusinessLayer.SearchBindingModels;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using log4net;
using LinqKit;

namespace BusinessLayer.Services
{
    public abstract class BaseService<TEntityVM, TEntity> where TEntity : Entity where TEntityVM : class, new()
    {
        protected readonly Repository<TEntity> Repository;
        private readonly IMapper _mapper;
        protected readonly ILog Log;

        protected BaseService(IUnitOfWork uow, IMapper mapper, ILog log)
        {
            _mapper = mapper;
            Log = log;
            Repository = new Repository<TEntity>(uow);
        }
       
        public IEnumerable<TEntityVM> GetEntities(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var entities = Repository.All
                    .OrderBy(entity => entity.Id)
                    .Skip(pageNumber*pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.SearchFilter())
                    .ToList();

                return entities.Select(entity => _mapper.Map<TEntityVM>(entity));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
            

        public abstract IEnumerable<TEntityVM> GetFullEntities(ISearchBindingModel<TEntity> searchParameters, int pageSize, int pageNumber);

        public TEntityVM Find(int id)
        {
            try
            {
                return _mapper.Map<TEntityVM>(Repository.Find(id));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public void InsertGraph(TEntityVM entityVMGraph)
        {
            try
            {
                Repository.InsertGraph(_mapper.Map<TEntity>(entityVMGraph));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public void InsertOrUpdate(TEntityVM entityVM)
        {
            try
            {
                Repository.InsertOrUpdate(_mapper.Map<TEntity>(entityVM));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
