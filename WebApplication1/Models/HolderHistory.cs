namespace Presentation.Models
{
    public class HolderHistory
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int HolderId { get; set; }
        public Holder Holder { get; set; }
        public DateTime MovedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public int ChangedById { get; set; }
        public Associate ChangedBy { get; set; }
    }
}
