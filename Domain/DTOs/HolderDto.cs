using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class HolderDto
    {
        public int Id { get; set; }
        public string? Abbreviation { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
    }
}
