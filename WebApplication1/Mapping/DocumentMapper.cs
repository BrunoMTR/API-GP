using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models.Forms;

namespace Presentation.Mapping
{
    public class DocumentMapper : IDocumentMapper
    {
        public DocumentFormDto Map(DocumentForm document)
        {
           return document is not null ? new DocumentFormDto
           {
               ProcessId = document.ProcessId,
               UploadedBy = document.UploadedBy,
               File = document.File
           } : null;
        }

        public DocumentForm Map(DocumentFormDto document)
        {
            return document is not null ? new DocumentForm
            {
                ProcessId = document.ProcessId,
                UploadedBy = document.UploadedBy,
                File = document.File
            } : null;
        }
    }
}
