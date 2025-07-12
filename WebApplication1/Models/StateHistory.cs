namespace Presentation.Models
{
    public class StateHistory
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int ChangedById { get; set; }
        public Associate ChangedBy { get; set; }

    }
}
