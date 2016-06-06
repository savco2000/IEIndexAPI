using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;
using Author = DataLayer.DomainModels.Author;

namespace BusinessLayer.Mappers
{
    public class ArticleVMToArticleMapper
    {
        public static Article Map(ArticleVM source)
        {
            var target = new Article
            {
                Title = source.Title,
                Page = source.Page,
                Issue = (Issues)Enum.Parse(typeof(Issues), source.Issue),
                PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), source.PublicationYear),
                IsSupplement = source.IsSupplement,
                Hyperlink = source.Hyperlink,
                Authors = (ICollection<Author>) source.Authors.Select(authorVM => new Author
                {
                    FirstName = authorVM.FirstName,
                    LastName = authorVM.LastName,
                    Suffix = (Suffixes)Enum.Parse(typeof(Suffixes), authorVM.Suffix)
                }),
                Subjects = (ICollection<Subject>) source.Subjects.Select(subjectVM => new Subject
                {
                    Name = subjectVM.Name
                })
            };

            return target;
        }

        public static Article Map(ArticleVM source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
