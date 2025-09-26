using BL.Handlers;
using Domain.Channels;
using Domain.DTOs;
using Domain.Repositories;
using Domain.Results;
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
        private readonly DocumentationHandler _documentationHandler;

        public DocumentationService(
            IFileStorageService fileStorage,
            IProcessRepository processRepository,
            IDocumentationRepository documentationRepository,
            IDocumentationChannel documentationChannel,
            IApplicationRepository applictionRepository,
            DocumentationHandler documentationHandler
            )
        {
            _processRepository = processRepository;
            _fileStorage = fileStorage;
            _documentationChannel = documentationChannel;
            _applictionRepository = applictionRepository;
            _documentationHandler = documentationHandler;
        }

        public async Task<Response<bool>> UploadAsync(
            DocumentFormDto document,
            CancellationToken cancellationToken)
        {
            if (document.File == null)
                return Response<bool>.Fail("No file was provided.");

            var process = await _processRepository.RetrieveAsync(document.ProcessId);
            if (process == null)
                return Response<bool>.Fail($"Process {document.ProcessId} not found.");

            if (process.Status == ProcessStatus.Concluded)
                return Response<bool>.Fail($"Process {document.ProcessId} is already concluded.");

            var application = await _applictionRepository.RetrieveAsync(process.ApplicationId);
            if (application == null)
                return Response<bool>.Fail($"Application {process.ApplicationId} not found.");

            var message = new DocumentMessageDto
            {
                ProcessId = document.ProcessId,
                ApplicationName = application.Abbreviation,
                TempFilePath = string.Empty,
                UploadedBy = document.UploadedBy,
                At = process.At
            };

            try
            {
                await _documentationHandler.QueueFileAsync(message, document.File, cancellationToken);
                return Response<bool>.Ok(true, "File successfully queued for processing.");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail($"Error while queuing file: {ex.Message}");
            }
        }

    }
}
