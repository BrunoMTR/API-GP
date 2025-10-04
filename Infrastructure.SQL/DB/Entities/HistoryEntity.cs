using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ProcessId { get; set; }
        public int At { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Notified { get; set; }

        public string? Note { get; set; }
        ProcessEntity Process { get; set; }
    }
}
