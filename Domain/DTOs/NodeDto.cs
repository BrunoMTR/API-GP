using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.DTOs
{
    public class NodeDto
    {

        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public int? Approvals { get; set; }
        public string Direction { get; set; }
    }
}
