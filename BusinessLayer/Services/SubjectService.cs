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
    public class SubjectService : BaseService<SubjectDTO, Subject>
    {
        private readonly IMapper _mapper;

        public SubjectService(GenericRepository<Subject> repository, IMapper mapper, ILog log) : base(repository, mapper, log)
        {
            _mapper = mapper;
        }

        public SubjectService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<SubjectDTO> GetEntities(Expression<Func<Subject, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
        {
            try
            {
                var query = Repository.All;

                query = orderDesc ? query.OrderByDescending(entity => entity.Name) : query.OrderBy(entity => entity.Name);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);

                var subjects = query.ToList();

                var subjectVms = subjects.Select(subject => _mapper.Map<SubjectDTO>(subject));

                return subjectVms;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public override IEnumerable<SubjectDTO> GetFullEntities(Expression<Func<Subject, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
        {
            try
            {
                var query = Repository.AllIncluding(subject => subject.Articles);

                query = orderDesc ? query.OrderByDescending(entity => entity.Name) : query.OrderBy(entity => entity.Name);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);

                var subjects = query.ToList();

                var subjectVms = subjects.Select(subject => _mapper.Map<SubjectDTO>(subject));

                return subjectVms;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
