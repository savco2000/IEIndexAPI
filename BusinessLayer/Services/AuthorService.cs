using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.ViewModels;
using DataLayer;
using DataLayer.DomainModels;
using LinqKit;

namespace BusinessLayer.Services
{
    public class AuthorService : IEIndexService<AuthorVM, Author>
    {
        public AuthorService(IUnitOfWork uow) : base(uow)
        {
        }

        public override List<AuthorVM> GetEntitiesWithChildren(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(author => author.LastName)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .Select(author => Mapper.Map<AuthorVM>(author))
                .ToList();

    }
}
