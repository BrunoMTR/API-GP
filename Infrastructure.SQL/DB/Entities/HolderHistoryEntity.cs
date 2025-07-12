using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class HolderHistoryEntity
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public ProcessEntity Process { get; set; }
        public int HolderId { get; set; }
        public HolderEntity Holder { get; set; }
        public DateTime MovedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public int ChangedById { get; set; }
        public AssociateEntity ChangedBy { get; set; }
    }
}
