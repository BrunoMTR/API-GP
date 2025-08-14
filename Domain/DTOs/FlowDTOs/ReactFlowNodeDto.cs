using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Flow
{
    public class ReactFlowNodeDto
    {
        public string Id { get; set; } = null!;   
        public PositionDto Position { get; set; } = null!; 
        public NodeDataDto Data { get; set; } = null!;
        public string Type { get; set; }
    }
}
