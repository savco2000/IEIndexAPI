using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class AuthorVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FullName => $"{FirstName} {LastName}, {Suffix}";
        public IList<ArticleVM> Articles { get; set; }

        public AuthorVM()
        {
            Articles = new List<ArticleVM>();
        }
    }
}
