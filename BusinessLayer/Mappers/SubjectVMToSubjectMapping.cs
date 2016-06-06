using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public class SubjectVMToSubjectMapping
    {
        public Subject Map(SubjectVM source)
        {
            var target = new Subject
            {
                Name = source.Name,
                Articles = (ICollection<Article>)source.Articles.Select(articleVM => new Article
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

        public Subject Map(SubjectVM source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}
