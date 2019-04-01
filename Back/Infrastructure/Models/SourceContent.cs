using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class SourceContent
    {
        public int Total { get; set; }

        public List<List<string>> Items { get; set; }
    }
}
