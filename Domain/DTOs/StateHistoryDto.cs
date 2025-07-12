using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    class StateHistoryDto
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public ProcessDto Process { get; set; }
        public int StateId { get; set; }
        public StateDto State { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int ChangedById { get; set; }
        public AssociateDto ChangedBy { get; set; }
    }
}
