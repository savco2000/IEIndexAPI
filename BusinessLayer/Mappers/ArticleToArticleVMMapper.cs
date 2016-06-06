using System;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public static class ArticleToArticleVMMapper
    {
        public static ArticleVM Map(Article source)
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
                    Suffix = author.Suffix.GetEnumDescription()
                }).ToList(),
                Subjects = source.Subjects.Select(subject => new SubjectVM { Name = subject.Name }).ToList()
            };

            return target;
        }

        public static ArticleVM Map(Article source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
