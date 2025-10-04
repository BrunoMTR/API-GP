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
        public string? Note { get; set; }
        public int At { get; set; }
        public int Approvals { get; set; }
        public ProcessStatus Status { get; set; }

        public List<HistoryDto> Histories { get; set; } = new List<HistoryDto>();
        public List<DocumentationDto> Documentations { get; set; } = new List<DocumentationDto>();

        public ApplicationDto Application { get; set; }
        public UnitDto Unit { get; set; }

    }

    public enum ProcessStatus
    {
        Initiated,
        Uploading,
        Pending,
        Concluded,
        Canceled
    }
}
