using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class ArticleVM
    {
        public string Title { get; set; }
        public int Page { get; set; }
        public string Issue { get; set; }
        public string PublicationYear { get; set; }
        public bool IsSupplement { get; set; }
        public string Hyperlink { get; set; }
        public IList<AuthorVM> Authors { get; set; }
        public IList<SubjectVM> Subjects { get; set; }

        public ArticleVM()
        {
            Authors = new List<AuthorVM>();
            Subjects = new List<SubjectVM>();
        }
    }
}
