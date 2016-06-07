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
    public class AuthorService : BaseService<AuthorVM, Author>
    {
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            _mapper = mapper;
        }

        public override IEnumerable<AuthorVM> GetEntitiesWithChildren(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) =>
            Repository.AllIncluding(x => x.Articles)
                .OrderBy(author => author.LastName)
                .Skip(pageNumber*pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .ToList()
                .Select(author => _mapper.Map<AuthorVM>(author));
    }
}
