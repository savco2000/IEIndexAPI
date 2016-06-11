using System.Linq;
using AutoMapper;
using BusinessLayer;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Contexts;
using log4net;
using Ninject;

namespace IEIndexConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = LogManager.GetLogger(typeof(Program));

            const int pageSize = 5;
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
                var articleService = new ArticleService(uow, mapper, log);
                var filter = new ArticleSearchFilter { Issue = "fall"};
                //var filter = new ArticleSearchFilter ();
                var articles = articleService.GetEntities(filter.SearchFilter(), pageSize: pageSize, pageNumber: pageNumber)
                    .ToList();
                //var articles = articleService.GetEntities(pageSize: pageSize, pageNumber: pageNumber)
                //    .ToList();
                var articlesWithChildren = articleService.GetFullEntities(filter, pageSize, pageNumber)
                    .ToList();
            }

            var context2 = kernel.Get<IEIndexContext>();
            //context2.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context2))
            {
                var authorService = new AuthorService(uow, mapper, log);
                var authors = authorService.GetEntities(pageSize: pageSize, pageNumber: pageNumber)
                    .ToList();
                var authorsWithChildren = authorService.GetFullEntities(new AuthorSearchFilter(), pageSize, pageNumber)
                    .ToList();
            }

            var context3 = kernel.Get<IEIndexContext>();
            //context3.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            using (var uow = new UnitOfWork<IEIndexContext>(context3))
            {
                var subjectService = new SubjectService(uow, mapper, log);
                var subjects = subjectService.GetEntities(pageSize: pageSize, pageNumber: pageNumber)
                    .ToList();
                var subjectsWithChildren = subjectService.GetFullEntities(new SubjectSearchFilter(), pageSize, pageNumber)
                    .ToList();
            }
        }
    }
}
