using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class FlowService : IFlowService
    {
        private readonly IFlowRepository _flowRepository;
        public FlowService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        public async Task<NormalizedNodeResponseDto?> Create(GraphDto graph)
        {
            var existingGraph = await _flowRepository.GetByApplicationIdAsync(graph.ApplicationId);
            if (existingGraph is not null)
            {
                return null;
            }

            var extendedNodes = new List<NodeDto>();

            foreach (var node in graph.Nodes)
            {
                extendedNodes.Add(new NodeDto
                {
                    OriginId = node.OriginId,
                    DestinationId = node.DestinationId,
                    Direction = "AVANÇO",
                    Approvals = node.Approvals
                });

                extendedNodes.Add(new NodeDto
                {
                    OriginId = node.DestinationId,
                    DestinationId = node.OriginId,
                    Direction = "RECUO",
                    Approvals = null
                });
            }

            var newGraph = new GraphDto
            {
                ApplicationId = graph.ApplicationId,
                Nodes = extendedNodes
            };

            return await _flowRepository.CreateAsync(newGraph);
        }




        public Task Delete(int applicationId)
        {
            throw new NotImplementedException();
        }

        public async Task<NormalizedNodeResponseDto?> Retrieve(int applicationId)
        {
            return await _flowRepository.GetByApplicationIdAsync(applicationId);
        }

        public async Task<ReactFlowDto?> RetrieveFlow(int applicationId)
        {
            return await _flowRepository.GetFlowByApplicationId(applicationId);
        }
    }
}
