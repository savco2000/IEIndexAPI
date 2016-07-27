using System.Collections.Generic;
using DataLayer.DomainModels;

namespace BusinessLayer.ViewModels
{
    public class AuthorVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FullName => string.IsNullOrWhiteSpace(Suffix) ? $"{FirstName} {LastName}" : $"{FirstName} {LastName}, {Suffix}";
        public IEnumerable<ArticleVM> Articles { get; set; }

        public AuthorVM()
        {
            Articles = new List<ArticleVM>();
        }
    }
}
