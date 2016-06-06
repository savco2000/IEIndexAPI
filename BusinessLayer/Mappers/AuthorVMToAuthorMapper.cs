using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class AuthorVMToAuthorMapper
    {
        public static Author Map(AuthorVM source)
        {
            var target = new Author
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Suffix = (Suffixes)Enum.Parse(typeof(Suffixes), source.Suffix),
                Articles = (ICollection<Article>) source.Articles.Select(articleVM => new Article
                {
                    Title = articleVM.Title,
                    Page = articleVM.Page,
                    Issue = (Issues)Enum.Parse(typeof(Issues), articleVM.Issue),
                    PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), articleVM.PublicationYear),
                    IsSupplement = articleVM.IsSupplement,
                    Hyperlink = articleVM.Hyperlink
                })
            };

            return target;
        }

        public static AuthorVM Map(Author source, params object[] options)
        {
            throw new NotImplementedException();
        }

    }
}
