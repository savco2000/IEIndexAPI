using System;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class AuthorToAuthorVMMapper : IMapToNew<Author, AuthorVM>
    {
        public AuthorVM Map(Author source)
        {
            var target = new AuthorVM
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Suffix = source.Suffix == Suffixes.Invalid ? null : source.Suffix.GetEnumDescription(),
                Articles = source.Articles.Select(article => new ArticleVM
                {
                    Title = article.Title,
                    Page = article.Page,
                    Issue = article.Issue == Issues.Invalid ? null : article.Issue.GetEnumDescription(),
                    PublicationYear = article.PublicationYear == PublicationYears.Invalid ? null : article.PublicationYear.GetEnumDescription(),
                    IsSupplement = article.IsSupplement,
                    Hyperlink = article.Hyperlink
                })
            };

            return target;
        }

        public AuthorVM Map(Author source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
