using System.Collections.Generic;
using System.Linq;
using BusinessLayer.BindingModels;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer
{
    public class IEIndexService
    {
        private readonly Repository<Article> _articleRepository;
        private Repository<Author> _authorRepository;
        private Repository<Subject> _subjectRepository;

        public IEIndexService(Repository<Article> articleRepository, Repository<Author> authorRepository, Repository<Subject> subjectRepository)
        {
            _articleRepository = articleRepository;
            _authorRepository = authorRepository;
            _subjectRepository = subjectRepository;
        }

        public List<Article> GetArticles(ArticleSearchBindingModel searchParameters, int pageSize, int pageNumber) => _articleRepository.All
                        .OrderBy(x => x.Title)
                        .Skip(pageNumber * pageSize - pageSize)
                        .Take(pageSize)
                        .AsExpandable()
                        .Where(searchParameters.GetPredicate())
                        .ToList();

        public List<Article> GetArticlesWithChildren(ArticleSearchBindingModel searchParameters, int pageSize, int pageNumber) => _articleRepository.AllIncluding(x => x.Authors, x => x.Subjects)
                        .OrderBy(x => x.Title)
                        .Skip(pageNumber * pageSize - pageSize)
                        .Take(pageSize)
                        .AsExpandable()
                        .Where(searchParameters.GetPredicate())
                        .ToList();

        public Article Find(int id) => _articleRepository.Find(id);

        public void InsertGraph(Article articleGraph) => _articleRepository.InsertGraph(articleGraph);

        public void InsertOrUpdate(Article article) => _articleRepository.InsertOrUpdate(article);

        public void Delete(int id) => _articleRepository.Delete(id);
    }
}
