using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DocumentationDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public int At { get; set; }
        public int ProcessId { get; set; }

    }
}
