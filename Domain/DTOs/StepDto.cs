using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StepDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public StateDto State { get; set; }
        public int HolderId { get; set; }
        public HolderDto Holder { get; set; }
        public bool IsFinal { get; set; }
        ICollection<StepHistoryDto> Executions { get; set; } = new List<StepHistoryDto>();
    }
}
