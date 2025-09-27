using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class ProcessEntity
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int At { get; set; }
        public int Approvals { get; set; }
        public ProcessStatus Status { get; set; }
        public ICollection<HistoryEntity> Histories { get; set; } = new List<HistoryEntity>();
        public ICollection<DocumentationEntity> Documentations { get; set; } = new List<DocumentationEntity>();
        public ApplicationEntity Application { get; set; }
        public UnitEntity Unit { get; set; }
        
    }
}
