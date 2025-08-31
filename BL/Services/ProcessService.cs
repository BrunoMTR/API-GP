using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
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

        public ProcessService(IProcessRepository processRepository, IFlowRepository flowRepository, IHistoryRepository historyRepository)
        {
            _processRepository = processRepository;
            _flowRepository = flowRepository;
            _historyRepository = historyRepository;
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



        public async Task<ProcessDto> Create(ProcessDto process)
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

            process.At = initialNode;

            // Criar o processo
            var createdProcess = await _processRepository.CreateAsync(process);

            return createdProcess;
        }

        public Task<List<ProcessDto>> GetAll()
        {
            return _processRepository.GetAllAsync();
        }

        public Task<List<ProcessDto>> GetAllByApplicationId(int applicationId)
        {
            return _processRepository.GetAllByApplicationIdAsync(applicationId);
        }

        public Task<ProcessDto> Retrieve(int id)
        {
            return _processRepository.RetrieveAsync(id);
        }
    }
}
