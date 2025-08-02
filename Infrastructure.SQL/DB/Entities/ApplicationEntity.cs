using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class ApplicationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Team { get; set; }
        public string? TeamEmail { get; set; }
        public string? ApplicationEmail { get; set; }

    }
}
