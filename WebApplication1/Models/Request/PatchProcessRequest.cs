using Microsoft.AspNetCore.Mvc;

namespace Presentation.Models.Request
{
    public class PatchProcessRequest
    {

        [FromForm(Name = "updatedBy")]
        public string UpdatedBy { get; set; }

        [FromForm(Name = "note")]
        public string? Note { get; set; } = string.Empty;

       
    }
}
