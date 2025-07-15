using Domain.DTOs;
using Infrastructure.SQL.DB.Entities;

namespace Presentation.Models
{
    public class StepHistory
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ExecutedById { get; set; }
        public Associate ExecutedBy { get; set; }
    }
}
