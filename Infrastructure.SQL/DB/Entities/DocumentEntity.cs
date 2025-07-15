using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ProcessId { get; set; }
        public ProcessEntity Process { get; set; }
        public int StepId { get; set; }
        public StepEntity Step { get; set; }
        public DateTime UploadedAt { get; set; }
        public int UploadedById { get; set; }
        public AssociateEntity UploadedBy { get; set; }
    }
}
