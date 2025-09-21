using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<NormalizedNodeResponseDto?> Create(ApplicationDto application, GraphDto graph)
        {
            // Verificar se o grafo original tem ciclo
            if (!GraphHasCycle(graph.Nodes))
            {
                return null; // fluxo inválido, não cria aplicação
            }

            var extendedNodes = new List<NodeDto>();
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var node = graph.Nodes[i];

                // Adiciona AVANÇO
                extendedNodes.Add(new NodeDto
                {
                    OriginId = node.OriginId,
                    DestinationId = node.DestinationId,
                    Direction = "AVANÇO",
                    Approvals = node.Approvals
                });

                // Adiciona RECUO apenas se não for o último node
                if (i < graph.Nodes.Count - 1)
                {
                    extendedNodes.Add(new NodeDto
                    {
                        OriginId = node.DestinationId,
                        DestinationId = node.OriginId,
                        Direction = "RECUO",
                        Approvals = 0
                    });
                }
            }

            var newGraph = new GraphDto
            {
                ApplicationId = application.Id,
                Nodes = extendedNodes
            };

            var newApplication = await _applicationRepository.CreateAsync(application, newGraph);
            return newApplication;
        }

        /// <summary>
        /// Verifica se o grafo contém pelo menos um ciclo
        /// </summary>
        private bool GraphHasCycle(List<NodeDto> nodes)
        {
            if (!nodes.Any())
                return false;

            // Mapear origem → destino
            var next = nodes.ToDictionary(n => n.OriginId, n => n.DestinationId);

            // Começar no primeiro origin
            int start = nodes.First().OriginId;
            int current = start;

            var visited = new HashSet<int> { start };

            while (next.ContainsKey(current))
            {
                current = next[current];
                if (current == start) return true; // ciclo fechado
                if (visited.Contains(current)) break; // visitado mas não voltou ao início → ciclo incompleto
                visited.Add(current);
            }

            return false; // não fechou ciclo
        }





        public async Task Delete(int id)
        {
             await _applicationRepository.DeleteAsync(id);
        }

        public async Task<List<ApplicationDto>> GetAll()
        {
            return await _applicationRepository.GetAllAsync();
        }

        public async Task<ApplicationDto> Retrieve(int id)
        {
            return await _applicationRepository.RetrieveAsync(id);
        }

        public async Task<ApplicationDto?> Update(ApplicationDto application, int applicationId)
        {
            var existingApplication = await _applicationRepository.RetrieveAsync(applicationId);
            if (existingApplication is null)
                return null;

            var allApplications = await _applicationRepository.GetAllAsync();

            var duplicated = allApplications.FirstOrDefault(a =>
                a.Id != applicationId && (
                    string.Equals(a.Name, application.Name, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.Abbreviation, application.Abbreviation, StringComparison.OrdinalIgnoreCase)
                ));

            if (duplicated is not null)
                return new ApplicationDto(); 

            return await _applicationRepository.UpdateAsync(application, applicationId);
        }

    }
}
