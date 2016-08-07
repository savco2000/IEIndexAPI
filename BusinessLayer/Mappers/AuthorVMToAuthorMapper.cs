using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class AuthorVMToAuthorMapper : IMapToNew<AuthorVM, Author>
    {
        public Author Map(AuthorVM source)
        {
            var target = new Author
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Suffix = (Suffixes)Enum.Parse(typeof(Suffixes), source.Suffix),
                Articles = source.Articles.Select(article => new Article
                {
                    Title = article.Title,
                    Page = article.Page,
                    Issue = (Issues) Enum.Parse(typeof(Issues), article.Issue),
                    PublicationYear = (PublicationYears) Enum.Parse(typeof(PublicationYears), article.PublicationYear),
                    IsSupplement = article.IsSupplement,
                    Hyperlink = article.Hyperlink
                }) as ICollection<Article>
            };

            return target;
        }

        public Author Map(AuthorVM source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
