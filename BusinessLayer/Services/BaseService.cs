using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.SearchFilters;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using log4net;

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

        public abstract IEnumerable<TEntityVM> GetEntities(Expression<Func<TEntity, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0);
        public abstract IEnumerable<TEntityVM> GetFullEntities(Expression<Func<TEntity, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0);

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
