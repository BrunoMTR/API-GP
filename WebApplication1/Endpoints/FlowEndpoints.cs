using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Endpoints
{
    public class FlowEndpoints
    {

        public static async Task<IResult> GetFlow(int applicationId,
           [FromServices] IFlowService flowService)
        {
            var flow = await flowService.RetrieveFlow(applicationId);
            if (flow is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(flow);
        }
    }
}
