using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.ViewModels;
using DataLayer;
using DataLayer.DomainModels;
using log4net;
using LinqKit;

namespace BusinessLayer.Services
{
    public class AuthorService : BaseService<AuthorVM, Author>
    {
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<AuthorVM> GetFullEntities(ISearchFilter<Author> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var authors = Repository.AllIncluding(x => x.Articles)
                    .OrderBy(author => author.LastName)
                    .Skip(pageNumber*pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.SearchFilter())
                    .ToList();

                return authors.Select(author => _mapper.Map<AuthorVM>(author));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
