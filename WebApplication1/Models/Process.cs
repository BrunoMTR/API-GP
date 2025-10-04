using Domain.DTOs;

namespace Presentation.Models
{
    public class Process
    {
        public int ApplicationId { get; set; }
        public string CreatedBy { get; set; }
        public string? Note { get; set; }
    }
}
