using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Mappers;
using BusinessLayer.SearchBindingModels;
using BusinessLayer.ViewModels;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.Repositories;
using LinqKit;

namespace BusinessLayer.Services
{
    public class NewIEIndexService
    {
        private readonly Repository<Article> _articleRepository;
        private readonly Repository<Author> _authorRepository;
        private readonly Repository<Subject> _subjectRepository;

        public NewIEIndexService(IUnitOfWork uow)
        {
            _articleRepository = new Repository<Article>(uow);
            _authorRepository = new Repository<Author>(uow);
            _subjectRepository = new Repository<Subject>(uow);
        }

        #region Articles

        public List<ArticleVM> GetArticles(ISearchBindingModel<Article> searchParameters, int pageSize, int pageNumber) => _articleRepository.All
                .OrderBy(x => x.Title)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .Select(article => ArticleToArticleVMMapper.Map(article))
                .ToList();

        public List<ArticleVM> GetArticlesWithChildren(ISearchBindingModel<Article> searchParameters, int pageSize, int pageNumber) => _articleRepository.AllIncluding(x => x.Authors, x => x.Subjects)
                .OrderBy(x => x.Title)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .Select(article => ArticleToArticleVMMapper.Map(article))
                .ToList();
       
        public ArticleVM Find(int id) => ArticleToArticleVMMapper.Map(_articleRepository.Find(id));
      
        public void InsertGraph(ArticleVM articleVMGraph) => _articleRepository.InsertGraph(ArticleVMToArticleMapper.Map(articleVMGraph));
        
        public void InsertOrUpdate(ArticleVM articleVM) => _articleRepository.InsertOrUpdate(ArticleVMToArticleMapper.Map(articleVM));
       
        public void Delete(int id) => _articleRepository.Delete(id);

        #endregion

        #region Authors

        public List<AuthorVM> GetAuthors(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) => _authorRepository.All
               .OrderBy(x => x.LastName)
               .Skip(pageNumber * pageSize - pageSize)
               .Take(pageSize)
               .AsExpandable()
               .Where(searchParameters.SearchFilter())
               .Select(author => AuthorToAuthorVMMapper.Map(author))
               .ToList();

        public List<AuthorVM> GetAuthorsWithChildren(ISearchBindingModel<Author> searchParameters, int pageSize, int pageNumber) => _authorRepository.AllIncluding(x => x.Articles)
                .OrderBy(x => x.LastName)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .AsExpandable()
                .Where(searchParameters.SearchFilter())
                .Select(author => AuthorToAuthorVMMapper.Map(author))
                .ToList();

        //public AuthorVM Find(int id) => AuthorToAuthorVMMapper.Map(_authorRepository.Find(id));

        public void InsertGraph(AuthorVM authorVMGraph) => _authorRepository.InsertGraph(AuthorVMToAuthorMapper.Map(authorVMGraph));

        public void InsertOrUpdate(AuthorVM authorVM) => _authorRepository.InsertOrUpdate(AuthorVMToAuthorMapper.Map(authorVM));

        //public void Delete(int id) => _articleRepository.Delete(id);

        #endregion

        #region Subjects

        #endregion
    }
}
