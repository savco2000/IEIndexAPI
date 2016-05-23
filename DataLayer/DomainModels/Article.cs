using System.Collections.Generic;

namespace DataLayer.DomainModels
{
    public sealed class Article : Entity
    {
        public string Title { get; set; }
        public int Page { get; set; }
        public Issues Issue { get; set; }
        public PublicationYears PublicationYear { get; set; }
        public bool IsSupplement { get; set; }
        public string Hyperlink { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<Subject> Subjects { get; set; }

        public Article()
        {
            Authors = new HashSet<Author>();
            Subjects = new HashSet<Subject>();
        }
    }
}
