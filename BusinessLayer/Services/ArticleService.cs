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
    public class ArticleService : BaseService<ArticleVM, Article>
    {
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<ArticleVM> GetEntities(Expression<Func<Article, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
        {
            try
            {
                var query = Repository.All;

                query = orderDesc ? query.OrderByDescending(entity => entity.Title) : query.OrderBy(entity => entity.Title);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);

                var entities = query.ToList();

                return entities.Select(entity => _mapper.Map<ArticleVM>(entity));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public override IEnumerable<ArticleVM> GetFullEntities(ISearchFilter<Article> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var articles = Repository.AllIncluding(x => x.Authors, x => x.Subjects)
                    .OrderBy(article => article.Title)
                    .Skip(pageNumber*pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.Filter())
                    .ToList();

                return articles.Select(article => _mapper.Map<ArticleVM>(article));
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
