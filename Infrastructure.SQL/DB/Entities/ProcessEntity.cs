using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class ProcessEntity
    {
        public int Id { get; set; }
        public string ProcessCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? Notes { get; set; }
        public int CurrentStepId { get; set; }
        public StepEntity CurrentStep { get; set; }
        public int CreatedById { get; set; }
        public AssociateEntity CreatedBy { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
        public ICollection<StepHistoryEntity> Executions { get; set; }
        public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();
    }
}
