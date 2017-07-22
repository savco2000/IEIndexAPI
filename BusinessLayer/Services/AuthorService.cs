using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs;
using DataLayer;
using DataLayer.DomainModels;
using log4net;
using LinqKit;

namespace BusinessLayer.Services
{
    public class AuthorService : BaseService<AuthorDTO, Author>
    {
        private readonly IMapper _mapper;

        public AuthorService(Repository<Author> repository, IMapper mapper, ILog log) 
            : base(repository, mapper, log)
                => _mapper = mapper;

        public AuthorService(IUnitOfWork uow, IMapper mapper, ILog log) 
            : base(uow, mapper, log)
                => _mapper = mapper;

        public override IEnumerable<AuthorDTO> GetEntities(Expression<Func<Author, bool>> searchFilter = null, 
            bool orderDesc = false, 
            int pageSize = 0, 
            int pageNumber = 0)
        {
            try
            {
                var query = Repository.All;

                query = orderDesc ? query.OrderByDescending(entity => entity.LastName) : query.OrderBy(entity => entity.LastName);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);                

                return query.Select(author => _mapper.Map<AuthorDTO>(author)).ToList();                
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public override IEnumerable<AuthorDTO> GetFullEntities(Expression<Func<Author, bool>> searchFilter = null, 
            bool orderDesc = false, 
            int pageSize = 0, 
            int pageNumber = 0)
        {
            try
            {
                var query = Repository.AllIncluding(x => x.Articles);

                query = orderDesc ? query.OrderByDescending(entity => entity.LastName) : query.OrderBy(entity => entity.LastName);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);                

                return query.Select(author => _mapper.Map<AuthorDTO>(author)).ToList();                
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
