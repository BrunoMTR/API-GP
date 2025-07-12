using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class StateHistoryEntity
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public ProcessEntity Process { get; set; }
        public int StateId { get; set; }
        public StateEntity State { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int ChangedById { get; set; }
        public AssociateEntity ChangedBy { get; set; }
    }
}
