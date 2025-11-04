using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.FlowDTOs
{
    public class NodeHistoryItem
    {
        public int At { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Notified { get; set; }
    }
}
