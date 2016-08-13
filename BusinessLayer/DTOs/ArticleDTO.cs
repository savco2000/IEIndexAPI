using System.Collections.Generic;

namespace BusinessLayer.DTOs
{
    public class ArticleDTO
    {
        public string Title { get; set; }
        public int Page { get; set; }
        public string Issue { get; set; }
        public string PublicationYear { get; set; }
        public bool IsSupplement { get; set; }
        public string Hyperlink { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Subjects { get; set; }

        public ArticleDTO()
        {
            Authors = new List<string>();
            Subjects = new List<string>();
        }
    }
}
