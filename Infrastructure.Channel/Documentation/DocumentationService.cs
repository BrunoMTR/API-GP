using Domain.Channels;
using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Channel.Documentation
{
    public class DocumentationService : BackgroundService
    {
        private readonly IDocumentationChannel _documentationChannel;
        private readonly IServiceProvider _serviceProvider;
        private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB

        public DocumentationService(IDocumentationChannel documentationChannel, IServiceProvider serviceProvider)
        {
            _documentationChannel = documentationChannel;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in _documentationChannel.ReadAllAsync(stoppingToken))
            {
                using var scope = _serviceProvider.CreateScope();
                var documentationRepository = scope.ServiceProvider.GetRequiredService<IDocumentationRepository>();
                var processRepository = scope.ServiceProvider.GetRequiredService<IProcessRepository>();
                var flowRepository = scope.ServiceProvider.GetRequiredService<IFlowRepository>();

                var fileName = Path.GetFileName(message.TempFilePath);
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploadFolder);

                // Calcular próximo número N
                var existingFiles = Directory.GetFiles(uploadFolder, $"{message.ApplicationName}-{message.ProcessId}-*");
                int nextNumber = existingFiles.Length + 1;

                // Novo nome final
                var finalFileName = $"{message.ApplicationName}-{message.ProcessId}-{nextNumber}.pdf";
                var finalPath = Path.Combine(uploadFolder, finalFileName);

                try
                {
                    bool isValid;

                    // Abrir stream apenas para validações
                    await using (var fs = new FileStream(message.TempFilePath, FileMode.Open, FileAccess.Read))
                    {
                        // 1️⃣ Validar tamanho
                        if (!ValidateFileSize(fs))
                        {
                            Console.WriteLine($"Ficheiro excede tamanho máximo ({MaxFileSize / (1024 * 1024)} MB): {fileName}");
                            continue;
                        }

                        // 2️⃣ Validar assinatura PDF
                        if (!ValidatePdf(fs))
                        {
                            Console.WriteLine($"Ficheiro inválido (não é PDF): {fileName}");
                            continue;
                        }

                        isValid = true;
                    } // stream fechado aqui

                    if (!isValid)
                        continue;

                    // 3️⃣ Mover para pasta final
                    File.Move(message.TempFilePath, finalPath);

                    var docDto = new DocumentationDto
                    {
                        FileName = finalFileName,
                        FilePath = finalPath,
                        FileSize = new FileInfo(finalPath).Length.ToString(),
                        FileType = "application/pdf",
                        UploadedAt = DateTime.UtcNow,
                        UploadedBy = message.UploadedBy,
                        At = message.At,
                        ProcessId = message.ProcessId
                    };

                    await documentationRepository.Upload(docDto);

                    // 4️⃣ Atualizar estado do processo
                    var process = await processRepository.RetrieveAsync(message.ProcessId);
                    if (process != null && process.Status == ProcessStatus.Uploading)
                    {
                        // Recupera o fluxo desta aplicação
                        var flow = await flowRepository.GetByApplicationAsync(process.ApplicationId);
                        var nodes = flow?.Nodes;

                        if (nodes != null && nodes.Any())
                        {
                            // Identificar OriginIds que aparecem com direção "RECUO"
                            var originIdsWithRecuo = nodes
                                .Where(n => n.Direction.Equals("RECUO", StringComparison.OrdinalIgnoreCase))
                                .Select(n => n.OriginId)
                                .Distinct()
                                .ToHashSet();

                            // O nó inicial é o OriginId que só aparece com direção "AVANÇO"
                            var initialNode = nodes
                                .Where(n => n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase))
                                .Select(n => n.OriginId)
                                .FirstOrDefault(id => !originIdsWithRecuo.Contains(id));

                            if (process.At == initialNode)
                            {
                                process.Status = ProcessStatus.Initiated;
                            }
                            else
                            {
                                process.Status = ProcessStatus.Pending;
                            }

                            await processRepository.UpdateAsync(process.Id, process);
                        }
                    }

                    Console.WriteLine($"Upload concluído: {finalPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro a processar {fileName}: {ex.Message}");
                }
            }
        }




        private bool ValidatePdf(Stream fileStream)
        {
            try
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] pdfSignature = new byte[5];
                int bytesRead = fileStream.Read(pdfSignature, 0, pdfSignature.Length);
                if (bytesRead < 5) return false;

                string sigHex = BitConverter.ToString(pdfSignature);
                return sigHex.StartsWith("25-50-44-46-2D"); // %PDF-
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateFileSize(Stream fileStream)
        {
            return fileStream.Length <= MaxFileSize;
        }


    }
}
