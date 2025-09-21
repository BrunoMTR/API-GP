namespace Presentation.Models
{
    public class Documentation
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public int At { get; set; }
    }
}
