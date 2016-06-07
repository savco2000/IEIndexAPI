using System.Linq;
using AutoMapper;
using BusinessLayer;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Contexts;
using Ninject;

namespace IEIndexConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            
            kernel.Bind<IMapper>().ToConstant(config.CreateMapper());

            var mapper = kernel.Get<IMapper>();

            var context1 = new IEIndexContext();
            //context1.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context1))
            {
                var articleService = new ArticleService(uow, mapper);
                var articles = articleService.GetEntities(new ArticleSearchBindingModel(), 10, 1)
                    .ToList();
                var articlesWithChildren = articleService.GetEntitiesWithChildren(new ArticleSearchBindingModel(), 10, 1)
                    .ToList();
            }

            var context2 = new IEIndexContext();
            //context2.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context2))
            {
                var authorService = new AuthorService(uow, mapper);
                var authors = authorService.GetEntities(new AuthorSearchBindingModel(), 10, 1)
                    .ToList();
                var authorsWithChildren = authorService.GetEntitiesWithChildren(new AuthorSearchBindingModel(), 10, 1)
                    .ToList();
            }

            var context3 = new IEIndexContext();
            //context3.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context3))
            {
                var subjectService = new SubjectService(uow, mapper);
                var subjects = subjectService.GetEntities(new SubjectSearchBindingModel(), 10, 1)
                    .ToList();
                var subjectsWithChildren = subjectService.GetEntitiesWithChildren(new SubjectSearchBindingModel(), 10, 1)
                    .ToList();
            }
        }
    }
}
