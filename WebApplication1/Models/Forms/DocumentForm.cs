using Microsoft.AspNetCore.Mvc;

namespace Presentation.Models.Forms
{
    public class DocumentForm
    {
        [FromForm(Name = "processId")]
        public int ProcessId { get; set; }

        [FromForm(Name = "createdBy")]
        public string UploadedBy { get; set; } = string.Empty;

        [FromForm(Name = "file")]
        public IFormFile? File { get; set; } = default!;
    }
}
