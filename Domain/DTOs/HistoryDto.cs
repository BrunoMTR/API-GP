using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ProcessId { get; set; }
        public int At { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
