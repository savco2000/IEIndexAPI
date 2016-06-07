﻿using System.Collections.Generic;
using System.Linq;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.ViewModels;
using DataLayer;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.Services
{
    public class ArticleService : BaseService<ArticleVM, Article>
    {
        public ArticleService(IUnitOfWork uow) : base(uow)
        {
        }

        public override List<ArticleVM> GetEntitiesWithChildren(ISearchBindingModel<Article> searchParameters, int pageSize, int pageNumber) => Repository.AllIncluding(x => x.Authors, x => x.Subjects)
                .OrderBy(article => article.Title)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .ToList()
                .Select(article => Mapper.Map<ArticleVM>(article))
                .ToList();
    }
}
