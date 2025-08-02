using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Mapping
{
    public class GraphMapper : IGraphMapper
    {
        public GraphDto Map(Graph graph)
        {
            return new GraphDto
            {
                ApplicationId = graph.ApplicationId,
                Nodes = graph.Nodes?.Select(n => new NodeDto
                {                
                    OriginId = n.OriginId,                  
                    DestinationId = n.DestinationId,                   
                    Approvals = n.Approvals,
                    Direction = n.Direction
                }).ToList()
            };
        }



        public Graph Map(GraphDto graphDto)
        {
            return new Graph
            {
                ApplicationId = graphDto.ApplicationId,
                Nodes = graphDto.Nodes?.Select(n => new Node
                {
                    OriginId = n.OriginId,
                    DestinationId = n.DestinationId,
                    Approvals = n.Approvals,
                    Direction = n.Direction
                }).ToList()
            };
        }
    }
}
