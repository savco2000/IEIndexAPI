using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class SubjectVMToSubjectMapper : IMapToNew<SubjectVM, Subject>
    {
        public Subject Map(SubjectVM source)
        {
            var target = new Subject
            {
                Name = source.Name,
                Articles = source.Articles.Select(article => new Article
                {
                    Title = article.Title,
                    Page = article.Page,
                    Issue = (Issues)Enum.Parse(typeof(Issues), article.Issue),
                    PublicationYear = (PublicationYears)Enum.Parse(typeof(PublicationYears), article.PublicationYear),
                    IsSupplement = article.IsSupplement,
                    Hyperlink = article.Hyperlink
                }) as ICollection<Article>
            };

            return target;
        }

        public Subject Map(SubjectVM source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
