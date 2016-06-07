using BusinessLayer.SearchBindingModels;
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Contexts;

namespace IEIndexConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var uow = new UnitOfWork<IEIndexContext>(new IEIndexContext()))
            {
                var articleService = new ArticleService(uow);

                var articles = articleService.GetEntities(new ArticleSearchBindingModel(), 10, 1);

                var articlesWithChildren = articleService.GetEntitiesWithChildren(new ArticleSearchBindingModel(), 10, 1);
            }
        }
    }
}
