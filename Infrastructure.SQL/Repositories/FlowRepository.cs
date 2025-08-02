using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Repositories
{
    public class FlowRepository : IFlowRepository

    {
        private readonly DemoContext _demoContext;

        public FlowRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public async Task<List<NodeDto>> CreateAsync(GraphDto graph)
        {
            var nodes = graph.Nodes.Select(node => new NodeEntity
            {
                ApplicationId = graph.ApplicationId,
                OriginId = node.OriginId,
                DestinationId = node.DestinationId,
                Approvals = node.Approvals,
                Direction = node.Direction
            }).ToList();

            _demoContext.Node.AddRange(nodes);
            await _demoContext.SaveChangesAsync();

            var savedNodes = nodes.Select(n => new NodeDto
            {
                Id = n.Id, 
                ApplicationId = n.ApplicationId,
                OriginId = n.OriginId,
                DestinationId = n.DestinationId,
                Approvals = n.Approvals,
                Direction = n.Direction
            }).ToList();

            return savedNodes;
        }


        public async Task DeleteByApplicationIdAsync(int applicationId)
        {
            await _demoContext.Node
                .Where(x => x.ApplicationId == applicationId)
                .ExecuteDeleteAsync();
        }

        public async Task<GraphDto> GetByApplicationIdAsync(int applicationId)
        {
             var nodes = await _demoContext.Node
                .AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .Select(x => new NodeDto
                {
                    Id = x.Id,
                    OriginId = x.OriginId,
                    DestinationId = x.DestinationId,
                    Approvals = x.Approvals,
                    Direction = x.Direction
                }).ToListAsync();

            if (!nodes.Any())
                return null;

            return new GraphDto { ApplicationId = applicationId, Nodes = nodes };

        }
    }
}
