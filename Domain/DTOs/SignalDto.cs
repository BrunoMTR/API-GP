using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SignalDto
    {
        public int ProcessId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
        public string? UploadedBy { get; set; }
        public string? Reason { get; set; }
    }
}
