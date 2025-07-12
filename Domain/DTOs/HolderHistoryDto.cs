using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    class HolderHistoryDto
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int HolderId { get; set; }
        public HolderDto Holder { get; set; }
        public DateTime MovedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public int ChangedById { get; set; }
        public AssociateDto ChangedBy { get; set; }
    }
}
