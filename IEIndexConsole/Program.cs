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

            using (var uow = new UnitOfWork<IEIndexContext>(new IEIndexContext()))
            {
                var mapper = kernel.Get<IMapper>();

                var articleService = new ArticleService(uow, mapper);

                var articles = articleService.GetEntities(new ArticleSearchBindingModel(), 10, 1);

                var articlesWithChildren = articleService.GetEntitiesWithChildren(new ArticleSearchBindingModel(), 10, 1);
            }
        }
    }
}
