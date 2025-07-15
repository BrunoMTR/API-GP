using Infrastructure.SQL.DB.Entities;

namespace Presentation.Models
{
    public class DocumentHistory
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public int PerformedById { get; set; }
        public Associate PerformedBy { get; set; }
        public string? Notes { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
    }
}
