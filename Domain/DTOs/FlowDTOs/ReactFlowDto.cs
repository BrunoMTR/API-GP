using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Flow
{
    public class ReactFlowDto
    {
        public List<ReactFlowNodeDto> Nodes { get; set; } = new();
        public List<ReactFlowEdgeDto> Edges { get; set; } = new();
    }
}
