using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class StepEntity
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public StateEntity State { get; set; }
        public int HolderId { get; set; }
        public HolderEntity Holder { get; set; }
        public bool IsFinal { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
        public ICollection<StepHistoryEntity> Executions { get; set; } = new List<StepHistoryEntity>();
    }
}
