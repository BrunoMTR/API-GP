using Infrastructure.SQL.DB.Entities;

namespace Presentation.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
        public DateTime? UploadedAt { get; set; }
        public int UploadedById { get; set; }
        public Associate UploadedBy { get; set; }
    }
}
