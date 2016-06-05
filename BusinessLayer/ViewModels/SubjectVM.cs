using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class SubjectVM
    {
        public string Name { get; set; }
        public IList<ArticleVM> Articles { get; set; }

        public SubjectVM()
        {
            Articles = new List<ArticleVM>();
        }
    }
}
