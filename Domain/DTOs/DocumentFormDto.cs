using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DocumentFormDto
    {
        public int ProcessId { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
        public IFormFile? File { get; set; } = default!;
    }
}
