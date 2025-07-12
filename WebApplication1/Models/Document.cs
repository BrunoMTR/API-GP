namespace Presentation.Models
{
    public class Document
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
    }
}
