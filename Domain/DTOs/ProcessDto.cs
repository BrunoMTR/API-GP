using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ProcessDto
    {
        public int Id { get; set; }
        public string ProcessCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? Notes { get; set; }
        public int CurrentStepId { get; set; }
        public StepDto CurrentStep { get; set; }
        public int CreatedById { get; set; }
        public AssociateDto CreatedBy { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationDto Application { get; set; }
        public ICollection<StepHistoryDto> Executions { get; set; }
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
