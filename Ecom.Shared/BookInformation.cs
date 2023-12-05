using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Shared
{
    public class BookInformation
    {
        public string? Title { get; set; }
        public string? FirstSubject { get; set; }
        public string? AuthorName { get; set; }
        public List<string>? AuthorNameList { get; set; } = new List<string>();
    }

    public class TotalWorksByDate
    {
        public DateTime Date { get; set; } = default!;
        public int CountOfWorks { get; set; }
    }
}
