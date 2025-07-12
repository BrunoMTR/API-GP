using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    class DocumetHistoryDto
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public DocumentDto Document { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public int PerformedById { get; set; }
        public AssociateDto PerformedBy { get; set; }
        public string? Notes { get; set; }
        public string? OldFileName { get; set; }
        public string? OldFilePath { get; set; }
    }
}
