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

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public ProcessStatus Status { get; set; }
        public ApplicationDto Application { get; set; } = new();
        public UnitDto Unit { get; set; } = new();
        public List<ReactFlowNodeDto> Nodes { get; set; } = new();
        public List<ReactFlowEdgeDto> Edges { get; set; } = new();
        
    }
}
