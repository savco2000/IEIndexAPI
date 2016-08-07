using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class ArticleVMToArticleMapper : IMapToNew<ArticleVM, Article>
    {
        public Article Map(ArticleVM source)
        {
            var target = new Article
            {
                Title = source.Title,
                Page = source.Page,
                Issue = (Issues)Enum.Parse(typeof(Issues), source.Issue),
                PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), source.PublicationYear),
                IsSupplement = source.IsSupplement,
                Hyperlink = source.Hyperlink,
                Authors = source.Authors.Select(author => new Author
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Suffix = (Suffixes) Enum.Parse(typeof(Suffixes), author.Suffix)
                }) as ICollection<Author>,
                Subjects = source.Subjects.Select(subject => new Subject { Name = subject.Name }) as ICollection<Subject>
            };

            return target;
        }

        public Article Map(ArticleVM source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
