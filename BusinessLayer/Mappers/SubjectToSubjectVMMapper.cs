using System;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class SubjectToSubjectVMMapper : IMapToNew<Subject, SubjectVM>
    {
        public SubjectVM Map(Subject source)
        {
            var target = new SubjectVM
            {
                Name = source.Name,
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

        public SubjectVM Map(Subject source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
