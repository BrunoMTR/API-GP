using Infrastructure.SQL.DB.Entities;

namespace Presentation.Models
{
    public class Process
    {
        public int Id { get; set; }
        public string ProcessCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? Notes { get; set; }
        public int CurrentStepId { get; set; }
        public Step CurrentStep { get; set; }
        public int CreatedById { get; set; }
        public Associate CreatedBy { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public ICollection<StepHistory> Executions { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
