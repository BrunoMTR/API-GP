using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class DocumentHistoryEntity
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public DocumentEntity Document { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public int PerformedById { get; set; }
        public AssociateEntity PerformedBy { get; set; }
        public string? Notes { get; set; }
        public string? OldFileName { get; set; }
        public string? OldFilePath { get; set; }
    }
}
