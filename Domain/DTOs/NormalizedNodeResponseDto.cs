using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class NormalizedNodeResponseDto
    {
        public ApplicationDto Application { get; set; }
        public List<UnitDto> Units { get; set; }
        public List<NodeDto> Nodes { get; set; }
    }
}
