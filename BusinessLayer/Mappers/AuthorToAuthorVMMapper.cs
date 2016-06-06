﻿using System;
using System.Linq;
using BusinessLayer.ViewModels;
using DataLayer.DomainModels;

namespace BusinessLayer.Mappers
{
    public static class AuthorToAuthorVMMapper
    {
        public static AuthorVM Map(Author source)
        {
            var target = new AuthorVM
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Suffix = source.Suffix.GetEnumDescription(),
                Articles = source.Articles.Select(article => new ArticleVM
                {
                    Title = article.Title,
                    Page = article.Page,
                    Issue = article.Issue.GetEnumDescription(),
                    PublicationYear = article.PublicationYear.GetEnumDescription(),
                    IsSupplement = article.IsSupplement,
                    Hyperlink = article.Hyperlink
                }).ToList()
            };

            return target;
        }

        public static AuthorVM Map(Author source, params object[] options)
        {
            throw new NotImplementedException();
        }
    }
}