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
    public class SubjectService : BaseService<SubjectVM, Subject>
    {
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<SubjectVM> GetFullEntities(ISearchBindingModel<Subject> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var subjects = Repository.AllIncluding(subject => subject.Articles)
                    .OrderBy(subject => subject.Name)
                    .Skip(pageNumber * pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.SearchFilter())
                    .ToList();

                return subjects.Select(article => _mapper.Map<SubjectVM>(article));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
