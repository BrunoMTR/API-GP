using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class NotificationEntity
    {
        public int Id { get; set; }
        public string To { get; set; }
        public List<string> Cc { get; set; } = new List<string>();
        public List<string>? Bcc { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime At { get; set; }
        public int ProcessId { get; set; }
    }
}
