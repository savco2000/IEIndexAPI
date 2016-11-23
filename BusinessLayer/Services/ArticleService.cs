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
    public class ArticleService : BaseService<ArticleDTO, Article>
    {
        private readonly IMapper _mapper;

        public ArticleService(GenericRepository<Article> repository, IMapper mapper, ILog log) : base(repository, mapper, log)
        {
            _mapper = mapper;
        }

        public ArticleService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<ArticleDTO> GetEntities(Expression<Func<Article, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
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

                var articles = query.ToList();

                var articleVms = articles.Select(author => _mapper.Map<ArticleDTO>(author));
                
                return articleVms;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public override IEnumerable<ArticleDTO> GetFullEntities(Expression<Func<Article, bool>> searchFilter = null, bool orderDesc = false, int pageSize = 0, int pageNumber = 0)
        {
            try
            {
                var query = Repository.AllIncluding(x => x.Authors, x => x.Subjects);

                query = orderDesc ? query.OrderByDescending(entity => entity.Title) : query.OrderBy(entity => entity.Title);

                if (pageSize != 0 && pageNumber != 0)
                    query = query.Skip(pageNumber * pageSize - pageSize).Take(pageSize);

                query = query.AsExpandable();

                if (searchFilter != null)
                    query = query.Where(searchFilter);

                var articles = query.ToList();
               
                var articleVms = articles.Select(author => _mapper.Map<ArticleDTO>(author));

                return articleVms;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
