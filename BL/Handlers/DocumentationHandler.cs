using Domain.Channels;
using Domain.DTOs;
using InfrastructureFileStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Handlers
{
    public class DocumentationHandler
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IDocumentationChannel _documentationChannel;

        public DocumentationHandler(
            IFileStorageService fileStorage,
            IDocumentationChannel documentationChannel)
        {
            _fileStorage = fileStorage;
            _documentationChannel = documentationChannel;
        }

        public async Task QueueFileAsync(DocumentMessageDto message, IFormFile file, CancellationToken cancellationToken)
        {
            var tempFilePath = await _fileStorage.SaveTempFileAsync(file, cancellationToken);

            message.TempFilePath = tempFilePath;

            await _documentationChannel.SubmitAsync(message, cancellationToken);
        }
    }
}
