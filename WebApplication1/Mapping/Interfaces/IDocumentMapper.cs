using Domain.DTOs;
using Presentation.Models.Forms;

namespace Presentation.Mapping.Interfaces
{
    public interface IDocumentMapper
    {
        DocumentFormDto Map(DocumentForm application);
        DocumentForm Map(DocumentFormDto application);
    }
}
