using System.Collections.Generic;

namespace BusinessLayer.DTOs
{
    public class SubjectDTO
    {
        public string Name { get; set; }
        public IEnumerable<string> Articles { get; set; }

        public SubjectDTO()
        {
            Articles = new List<string>();
        }
    }
}
