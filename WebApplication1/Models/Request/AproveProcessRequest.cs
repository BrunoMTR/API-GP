using Microsoft.AspNetCore.Mvc;

namespace Presentation.Models.Request
{
    public class AproveProcessRequest
    {
        [FromForm(Name = "processId")]
        public int ProcessId { get; set; }

        [FromForm(Name = "updatedBy")]
        public string UpdatedBy { get; set; }

        [FromForm(Name = "note")]
        public string? Note { get; set; } = string.Empty;

        [FromForm(Name = "file")]
        public IFormFile? File { get; set; } = default!;
    }
}
