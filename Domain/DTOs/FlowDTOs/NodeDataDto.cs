using Domain.DTOs.FlowDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Flow
{
    public class NodeDataDto
    {
        public string Label { get; set; } = null!;
        public string? Status { get; set; } = null!;

        public string? Email { get; set; }
        public int? Approvals { get; set; }

        public List<NodeHistoryItem>? History { get; set; } = new();
    }
}
