using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ProcessId { get; set; }
        public ProcessDto Process { get; set; }
        public int StepId { get; set; }
        public StepDto Step { get; set; }
        public DateTime UploadedAt { get; set; }
        public int UploadedById { get; set; }
        public AssociateDto UploadedBy { get; set; }
    }
}
