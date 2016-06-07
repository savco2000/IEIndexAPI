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
            const int pageSize = 10;
            const int pageNumber = 1;

            var kernel = new StandardKernel();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            
            kernel.Bind<IMapper>().ToConstant(config.CreateMapper());
            kernel.Bind<IEIndexContext>().To<IEIndexContext>().InTransientScope();

            var mapper = kernel.Get<IMapper>();

            var context1 = kernel.Get<IEIndexContext>();
            //context1.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context1))
            {
                var articleService = new ArticleService(uow, mapper);
                var filter = new ArticleSearchBindingModel { Issue = "fall", PublicationYear = "2016"};
                //var filter = new ArticleSearchBindingModel ();
                var articles = articleService.GetEntities(filter, pageSize, pageNumber)
                    .ToList();
                var articlesWithChildren = articleService.GetEntitiesWithChildren(filter, pageSize, pageNumber)
                    .ToList();
            }

            var context2 = kernel.Get<IEIndexContext>();
            //context2.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context2))
            {
                var authorService = new AuthorService(uow, mapper);
                var authors = authorService.GetEntities(new AuthorSearchBindingModel(), pageSize, 1)
                    .ToList();
                var authorsWithChildren = authorService.GetEntitiesWithChildren(new AuthorSearchBindingModel(), pageSize, pageNumber)
                    .ToList();
            }

            var context3 = kernel.Get<IEIndexContext>();
            //context3.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context3))
            {
                var subjectService = new SubjectService(uow, mapper);
                var subjects = subjectService.GetEntities(new SubjectSearchBindingModel(), pageSize, pageNumber)
                    .ToList();
                var subjectsWithChildren = subjectService.GetEntitiesWithChildren(new SubjectSearchBindingModel(), pageSize, pageNumber)
                    .ToList();
            }
        }
    }
}
