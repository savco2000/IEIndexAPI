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
    public class SubjectService : BaseService<SubjectVM, Subject>
    {
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            _mapper = mapper;
        }

        public override List<SubjectVM> GetEntitiesWithChildren(ISearchBindingModel<Subject> searchParameters, int pageSize, int pageNumber) => 
            Repository.AllIncluding(subject => subject.Articles)
                .OrderBy(subject => subject.Articles)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .Select(article => _mapper.Map<SubjectVM>(article))
                .ToList();
    }
}
