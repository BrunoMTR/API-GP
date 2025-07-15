using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StepHistoryDto
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public ProcessDto Process { get; set; }
        public int StepId { get; set; }
        public StepDto Step { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ExecutedById { get; set; }
        public AssociateDto ExecutedBy { get; set; }
    }
}
