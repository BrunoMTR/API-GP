using Domain.Channels;
using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.DTOs.FlowDTOs;
using Domain.Repositories;
using Domain.Services;
using InfrastructureFileStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BL.Services
{
    public class ProcessService : IProcessService
    {
        private IProcessRepository _processRepository;

        private IFlowRepository _flowRepository;
        private IHistoryRepository _historyRepository;
        private IApplicationRepository _applicationRepository;
        private IDocumentationChannel _documentationChannel;
        private readonly IFileStorageService _fileStorage;

        public ProcessService(IProcessRepository processRepository, IFlowRepository flowRepository, IHistoryRepository historyRepository, IApplicationRepository applicationRepository, IDocumentationChannel documentationChannel, IFileStorageService fileStorage)
        {
            _processRepository = processRepository;
            _flowRepository = flowRepository;
            _historyRepository = historyRepository;
            _applicationRepository = applicationRepository;
            _documentationChannel = documentationChannel;
            _fileStorage = fileStorage;
        }


        public async Task<ProcessDto> Approve(int processId, string updatedBy)
        {
            // --- Recupera processo ---
            var process = await _processRepository.RetrieveAsync(processId);
            if (process == null)
                throw new Exception("Process not found.");

            if (process.Status == ProcessStatus.Concluded)
                throw new Exception("Process already concluded.");

            if (process.Status != ProcessStatus.Pending && process.Status != ProcessStatus.Initiated)
                throw new Exception("Only pending or initiated processes can be approved.");

            // --- Recupera fluxo ---
            var flow = await _flowRepository.GetByApplicationIdAsync(process.ApplicationId);
            var nodes = flow?.Nodes;
            if (flow == null || nodes == null)
                throw new Exception("Flow not found.");

            // Nó atual
            var currentNode = nodes.FirstOrDefault(n =>
                n.OriginId == process.At &&
                n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase));

            if (currentNode == null)
                throw new Exception("Current node not found in flow or no forward node available.");

            // Nó inicial = aquele cujo OriginId aparece apenas uma vez
            var initialNode = nodes
                .GroupBy(n => n.OriginId)
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .FirstOrDefault();

            if (initialNode == null)
                throw new Exception("Initial node not found.");

            // Total de aprovações necessárias para percorrer todo o fluxo (todos AVANÇO)
            int totalFlowApprovals = nodes
                .Where(n => n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase))
                .Sum(n => n.Approvals);

            int requiredApprovals = currentNode.Approvals;
            bool willTransition = (process.Approvals + 1) >= requiredApprovals;
            int nextNodeId = currentNode.DestinationId;

            // --- Salvar histórico ANTES de atualizar ---
            var historyDto = new HistoryDto
            {
                ApplicationId = process.ApplicationId,
                ProcessId = process.Id,
                At = process.At,                   // onde estava
                UpdatedBy = updatedBy,             // quem aprovou
                UpdatedAt = DateTime.UtcNow        // quando
            };
            await _historyRepository.CreateAsync(historyDto);

            // --- Atualização do processo ---
            if (willTransition)
            {
                // Completa quorum → avança
                process.Approvals = 0; // zera contagem do nó atual
                process.At = nextNodeId;

                // Retornou ao initial node → Concluded
                if (nextNodeId == initialNode.OriginId)
                {
                    process.Status = ProcessStatus.Concluded;
                    process.Approvals = totalFlowApprovals;
                }
                // Saiu do initial node → vira Pending
                else if (process.Status == ProcessStatus.Initiated)
                {
                    process.Status = ProcessStatus.Pending;
                }
            }
            else
            {
                // Ainda não completou quorum → só acumula
                process.Approvals += 1;
            }

            return await _processRepository.UpdateAsync(processId, process);
        }



        public async Task<ProcessDto> Create(ProcessDto process, bool uploading)
        {
            // Buscar nodes do fluxo da aplicação
            var normalizedNodes = await _flowRepository.GetByApplicationIdAsync(process.ApplicationId);

            if (normalizedNodes == null || !normalizedNodes.Nodes.Any())
                throw new Exception("No application found.");

            var nodes = normalizedNodes.Nodes;

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

            if (initialNode == 0)
                throw new Exception("Cannot determine the initial node for this application.");

            // Encontrar a relação de avanço a partir do initialNode
            var initialEdge = nodes.FirstOrDefault(n =>
                n.OriginId == initialNode &&
                n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase));

            if (initialEdge == null)
                throw new Exception("Initial node has no outgoing 'AVANÇO' edge.");

            // --- Definição de At e Status ---

            if (initialEdge.Approvals == 0)
            {
                // Sem approvals → salta logo
                process.At = initialEdge.DestinationId;
                process.Status = ProcessStatus.Pending;
            }
            else
            {
                // Precisa approvals → fica no inicial
                process.At = initialNode;
                process.Status = ProcessStatus.Initiated;
            }

            if (uploading)
            {
                process.Status = ProcessStatus.Uploading;
            }

            // Criar o processo
            var createdProcess = await _processRepository.CreateAsync(process);

            return createdProcess;
        }

        public Task<List<ProcessFlowDto>> GetAllAsync(ProcessQueryParams query)
        {
            return _processRepository.GetAllAsync(query);
        }

        public Task<List<ProcessDto>> GetAllByApplicationId(int applicationId)
        {
            return _processRepository.GetAllByApplicationIdAsync(applicationId);
        }

        public async Task<ProcessFlowDto?> GetProcessFlow(int processId)
        {
            var flow = await _flowRepository.GetFlowByApplicationId(1);
            return await _processRepository.GetProcessFlow(processId, flow);
        }

        public async Task<ProcessDto> CreateWithFileAsync(
        ProcessDto process,
        IFormFile? file,
        CancellationToken cancellationToken)
        {
            // 1. Criação do processo

            // 2. Só continua se recebeu ficheiro
            if (file is null)
            {
                var createdProcessWithoutFile = await Create(process, false);
                return createdProcessWithoutFile;
            }

            var createdProcessWithFile = await Create(process, true);

            // 3. Buscar a aplicação
            var application = await _applicationRepository.RetrieveAsync(process.ApplicationId);
            if (application is null)
                throw new Exception($"Application {process.ApplicationId} not found.");

            // 4. Criar mensagem
            var message = new DocumentMessageDto
            {
                ProcessId = createdProcessWithFile.Id,
                ApplicationName = application.Abbreviation,
                TempFilePath = string.Empty,
                UploadedBy = process.CreatedBy,
                At = process.At

            };

            // 5. Tratar upload
            await HandleFileUploadAsync(message, file, cancellationToken);

            return createdProcessWithFile;
        }


        public async Task HandleFileUploadAsync(DocumentMessageDto message, IFormFile file, CancellationToken cancellationToken)
        {

            var tempFilePath = await _fileStorage.SaveTempFileAsync(file, cancellationToken);

            message.TempFilePath = tempFilePath;

            await _documentationChannel.SubmitAsync(message, cancellationToken);

        }
    }
}
