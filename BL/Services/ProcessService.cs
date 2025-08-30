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

        public ProcessService(IProcessRepository processRepository, IFlowRepository flowRepository)
        {
            _processRepository = processRepository;
            _flowRepository = flowRepository;
        }

        public async Task<ProcessDto> Approve(int processId)
        {
            var process = await _processRepository.RetrieveAsync(processId);
            if (process == null)
                throw new Exception("Process not found.");

            // Aceita apenas Pending ou Initiated
            if (process.Status != ProcessStatus.Pending && process.Status != ProcessStatus.Initiated)
                throw new Exception("Only pending or initiated processes can be approved.");

            var flow = await _flowRepository.GetByApplicationIdAsync(process.ApplicationId);
            if (flow == null || flow.Nodes == null)
                throw new Exception("Flow not found.");

            // Pega o currentNode apenas com direção "AVANÇO"
            var currentNode = flow.Nodes
                .FirstOrDefault(n => n.OriginId == process.At
                                     && n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase));

            if (currentNode == null)
                throw new Exception("Current node not found in flow or no forward node available.");

            int requiredApprovals = currentNode.Approvals;

            if (process.Approvals + 1 >= requiredApprovals)
            {
                process.Approvals = 0;

                // Avança para o destinationId do currentNode
                process.At = currentNode.DestinationId;

                // Se estava inicializado, passa para Pending
                if (process.Status == ProcessStatus.Initiated)
                    process.Status = ProcessStatus.Pending;

                // Identifica o node inicial do fluxo (aquele que não é destino de nenhum node de recuo)
                var initialNode = flow.Nodes
                    .Where(n => !flow.Nodes.Any(x => x.DestinationId == n.OriginId
                                                     && x.Direction.Equals("RECUO", StringComparison.OrdinalIgnoreCase)))
                    .FirstOrDefault();

                // Se o destinationId do currentNode aponta para o node inicial → concluído
                if (initialNode != null && currentNode.DestinationId == initialNode.OriginId)
                {
                    process.Status = ProcessStatus.Concluded;
                    process.At = currentNode.DestinationId;
                }
            }
            else
            {
                process.Approvals += 1;

                if (process.Status == ProcessStatus.Initiated)
                    process.Status = ProcessStatus.Pending;
            }

            var updatedProcess = await _processRepository.UpdateAsync(processId, process);
            return updatedProcess;
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
