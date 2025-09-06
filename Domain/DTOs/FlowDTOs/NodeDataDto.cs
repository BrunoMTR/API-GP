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
    }
}
