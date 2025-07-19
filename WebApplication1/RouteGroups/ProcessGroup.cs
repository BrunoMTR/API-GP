

using Microsoft.AspNetCore.Http;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;
using Presentation.Models.Form;

namespace Presentation.RouteGroups
{
    public static class ProcessGroup
    {
        public static void AddProcessEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/processes");
            group.MapPost("/", ProcessEndpoints.PostProcess)
                .AddEndpointFilter<InputValidatorFilter<ProcessForm>>()
                .DisableAntiforgery(); 

            group.MapGet("/", ProcessEndpoints.GetProcess);

            group.MapGet("/{id}", ProcessEndpoints.Retrive)
                .WithName("processById");
                

        }

        
    }
}
