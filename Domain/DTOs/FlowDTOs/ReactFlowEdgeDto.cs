using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Flow
{
    public class ReactFlowEdgeDto
    {
        public string Id { get; set; } = null!;    
        public string Source { get; set; } = null!;  
        public string Target { get; set; } = null!;  
        public string? Label { get; set; }
    }
}
