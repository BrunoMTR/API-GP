using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DocumentMessageDto
    {
        public string TempFilePath { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public int ProcessId { get; set; }
        public int At { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
    }
}
