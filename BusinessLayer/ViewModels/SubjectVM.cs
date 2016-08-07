using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class SubjectVM
    {
        public string Name { get; set; }
        public IEnumerable<ArticleVM> Articles { get; set; }

        public SubjectVM()
        {
            Articles = new List<ArticleVM>();
        }
    }
}
