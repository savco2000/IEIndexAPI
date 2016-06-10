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
    public class ArticleService : BaseService<ArticleVM, Article>
    {
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork uow, IMapper mapper, ILog log) : base(uow, mapper, log)
        {
            _mapper = mapper;
        }

        public override IEnumerable<ArticleVM> GetFullEntities(ISearchBindingModel<Article> searchParameters, int pageSize, int pageNumber)
        {
            try
            {
                var articles = Repository.AllIncluding(x => x.Authors, x => x.Subjects)
                    .OrderBy(article => article.Title)
                    .Skip(pageNumber*pageSize - pageSize)
                    .Take(pageSize)
                    .AsExpandable()
                    .Where(searchParameters.SearchFilter())
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
