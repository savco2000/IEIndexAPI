using System.Collections.Generic;

namespace DataLayer.DomainModels
{
    public sealed class Author : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Suffixes Suffix { get; set; }
        public ICollection<Article> Articles { get; set; }

        public Author()
        {
            Articles = new HashSet<Article>();
        }
    }
}
