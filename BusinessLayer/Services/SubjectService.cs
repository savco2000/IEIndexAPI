﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.SearchFilters;
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

        public override IEnumerable<SubjectVM> GetEntities(Expression<Func<Subject, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
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

                var entities = query.ToList();

                return entities.Select(entity => _mapper.Map<SubjectVM>(entity));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public override IEnumerable<SubjectVM> GetFullEntities(ISearchFilter<Subject> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var subjects = Repository.AllIncluding(subject => subject.Articles)
                    .OrderBy(subject => subject.Name)
                    .Skip(pageNumber * pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.Filter())
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
