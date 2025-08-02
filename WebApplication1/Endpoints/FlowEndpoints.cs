using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Endpoints
{
    public class FlowEndpoints
    {
        public static async Task<IResult> PostFlow([FromBody] Graph graph, 
            [FromServices] IGraphMapper mapper,
            [FromServices] IFlowService flowService)
        {
            var graphDto = mapper.Map(graph);
            var nodes = await flowService.Create(graphDto);
            if(nodes is null)
                return Results.Conflict(new
                {
                    message = "Já existe fluxo para este workflow."
                });
            return Results.CreatedAtRoute(
                routeName: "flowByApplicationId",
                routeValues: new { applicationId = graph.ApplicationId },
                value: nodes
            );
        }

        public static async Task<IResult> GetFlow(int applicationId,
            [FromServices] IFlowService flowService,
            [FromServices] IGraphMapper mapper)
        {
            var graph = await flowService.Retrieve(applicationId);
            if(graph is null)
            {
                return Results.NotFound();
            }
            var graphDto = mapper.Map(graph);
            return Results.Ok(graphDto);
        }
    }
}
