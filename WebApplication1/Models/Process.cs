namespace Presentation.Models
{
    public class Process
    {
        public int Id { get; set; }
        public string ProcessCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }   
        public string? Notes { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public int CreatedById { get; set; }
        public Associate CreatedBy { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public int HolderId { get; set; }
        public Holder Holder { get; set; }
        public  ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
