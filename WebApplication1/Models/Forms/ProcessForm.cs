using Microsoft.AspNetCore.Mvc;

namespace Presentation.Models.Forms
{
    public class ProcessForm
    {
        [FromForm(Name = "applicationId")]
        public int ApplicationId { get; set; }

        [FromForm(Name = "createdBy")]
        public string CreatedBy { get; set; } = string.Empty;

        [FromForm(Name = "note")]
        public string? Note { get; set; } = string.Empty;

        [FromForm(Name = "file")]
        public IFormFile? File { get; set; } = default!;

    }
}
