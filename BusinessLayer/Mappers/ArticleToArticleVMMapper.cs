using System;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class ArticleToArticleVMMapper : IMapToNew<Article, ArticleVM>
    {
        public ArticleVM Map(Article source)
        {
            var target = new ArticleVM
            {
                Title = source.Title,
                Page = source.Page,
                Issue = source.Issue.GetEnumDescription(),
                PublicationYear = source.PublicationYear.GetEnumDescription(),
                IsSupplement = source.IsSupplement,
                Hyperlink = source.Hyperlink,
                Authors = source.Authors.Select(author => new AuthorVM
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Suffix = author.Suffix == Suffixes.Invalid ? null : author.Suffix.GetEnumDescription()
                }),
                Subjects = source.Subjects.Select(x => new SubjectVM { Name = x.Name })
            };

            return target;
        }

        public ArticleVM Map(Article source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
