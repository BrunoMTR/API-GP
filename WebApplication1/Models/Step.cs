using Domain.DTOs;
using Infrastructure.SQL.DB.Entities;

namespace Presentation.Models
{
    public class Step
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public int HolderId { get; set; }
        public Holder Holder { get; set; }
        public bool IsFinal { get; set; }
        ICollection<StepHistory> Executions { get; set; } = new List<StepHistory>();
    }
}
