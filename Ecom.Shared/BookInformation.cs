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
        public List<string>? Subjects { get; set; }
    }

    public class TotalWorksByDate
    {
        public DateTime Date { get; set; } = default!;
        public int CountOfWorks { get; set; }
    }
}
