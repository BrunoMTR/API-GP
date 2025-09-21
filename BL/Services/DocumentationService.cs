using Domain.Channels;
using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using InfrastructureFileStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Services
{
    public class DocumentationService : IDocumentationService
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IProcessRepository _processRepository;
        private readonly IDocumentationChannel _documentationChannel;
        private readonly IApplicationRepository _applictionRepository;

        public DocumentationService(
            IFileStorageService fileStorage,
            IProcessRepository processRepository,
            IDocumentationRepository documentationRepository,
            IDocumentationChannel documentationChannel,
            IApplicationRepository applictionRepository)
        {
            _processRepository = processRepository;
            _fileStorage = fileStorage;
            _documentationChannel = documentationChannel;
            _applictionRepository = applictionRepository;
        }

        public async Task UploadFile(
            DocumentFormDto document,
            CancellationToken cancellationToken)
        {
            if (document.File == null)
                throw new ArgumentNullException(nameof(document.File), "Nenhum ficheiro foi enviado.");

            // 1. Recuperar o processo
            var process = await _processRepository.RetrieveAsync(document.ProcessId);
            if (process == null)
                throw new Exception($"Processo {document.ProcessId} não encontrado.");

            if(process.Status == ProcessStatus.Concluded)
                throw new Exception($"Processo {document.ProcessId} concluído.");


            var application = await _applictionRepository.RetrieveAsync(process.ApplicationId);

            if(application == null)
                throw new Exception($"Application {process.ApplicationId} não encontrada.");
            // 2. Criar mensagem para o canal
            var message = new DocumentMessageDto
            {
                ProcessId = document.ProcessId,
                ApplicationName = application.Abbreviation,
                TempFilePath = string.Empty,
                UploadedBy = document.UploadedBy,
                At = process.At
            };

            // 3. Tratar upload físico + enviar ao canal
            await HandleFileUploadAsync(message, document.File, cancellationToken);

    
        }

        public async Task HandleFileUploadAsync(DocumentMessageDto message, IFormFile file, CancellationToken cancellationToken)
        {
            var tempFilePath = await _fileStorage.SaveTempFileAsync(file, cancellationToken);
            message.TempFilePath = tempFilePath;

            if (_documentationChannel != null)
                await _documentationChannel.SubmitAsync(message, cancellationToken);
        }
    }
}
