using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class DocumentationEntity
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
        ProcessEntity Process { get; set; }
    }
}
