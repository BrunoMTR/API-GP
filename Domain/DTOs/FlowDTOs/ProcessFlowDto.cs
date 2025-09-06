using Domain.DTOs.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.FlowDTOs
{
    public class ProcessFlowDto
    {

        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string At { get; set; }
        public string Workflows { get; set; }
        public string Status { get; set; }
        public List<ReactFlowNodeDto> Nodes { get; set; } = new();
        public List<ReactFlowEdgeDto> Edges { get; set; } = new();
    }
}
