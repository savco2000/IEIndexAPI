using System.Collections.Generic;

namespace DataLayer.DomainModels
{
    public class Subject : Entity
    {
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }

        public Subject()
        {
            Articles = new HashSet<Article>();
        }
    }
}
