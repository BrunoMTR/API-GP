using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class Query
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public int? ApplicationId { get; set; }
        public string? DateFilter { get; set; } // "last7", "last30", "all"
    }
}
