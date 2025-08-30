using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ProcessDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int At { get; set; }
        public int Approvals { get; set; }
        public ProcessStatus Status { get; set; }
    }

    public enum ProcessStatus
    {
        Initiated,
        Pending,
        Concluded,
        Canceled
    }
}
