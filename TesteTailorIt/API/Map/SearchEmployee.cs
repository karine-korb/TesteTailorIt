using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Map
{
    public class SearchEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public int IdHability { get; set; }
        public string NameHability { get; set; }
    }
}
