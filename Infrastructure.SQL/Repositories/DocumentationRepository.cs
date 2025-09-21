using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.SQL.Repositories
{
    public class DocumentationRepository : IDocumentationRepository
    {
        private readonly DemoContext _demoContext;
        public DocumentationRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }
        public async Task<DocumentationDto> Upload(DocumentationDto documentation)
        {
            var documentationEntity = new DocumentationEntity
            {
                FileName = documentation.FileName,
                FilePath = documentation.FilePath,
                FileSize = documentation.FileSize,
                FileType = documentation.FileType,
                UploadedAt = documentation.UploadedAt,
                UploadedBy = documentation.UploadedBy,
                At = documentation.At,
                ProcessId = documentation.ProcessId
            };

            await _demoContext.Documentation.AddAsync(documentationEntity);
            await _demoContext.SaveChangesAsync();
            documentation.Id = documentationEntity.Id;
            return documentation;
        }
    }
}
