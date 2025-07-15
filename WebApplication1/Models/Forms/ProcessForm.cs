namespace Presentation.Models.Form
{
    public class ProcessForm
    {
        public string ProcessCode { get; set; }
        public string? Notes { get; set; }
        public int CreatedById { get; set; }
        public int ApplicationId { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
