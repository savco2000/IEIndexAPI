using System.Collections.Generic;

namespace BusinessLayer.DTOs
{
    public class AuthorDTO
    {
        public string FullName { get; set; }
        public IEnumerable<string> Articles { get; set; }

        public AuthorDTO()
        {
            Articles = new List<string>();
        }
    }
}
