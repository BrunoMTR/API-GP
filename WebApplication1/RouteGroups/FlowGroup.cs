using Asp.Versioning;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;
using Presentation.Validations;

namespace Presentation.RouteGroups
{
    public static class FlowGroup
    {
        public static void AddFlowGroup(this WebApplication app)
        {
            var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();
            var group = app.MapGroup("flows").WithTags("Flows").WithApiVersionSet(versionSet).HasApiVersion(1.0);
            
            group.MapPost("/", FlowEndpoints.PostFlow)
                .AddEndpointFilter<InputValidatorFilter<Graph>>()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create a new application flow",
                    Description = "Creates a new nodes entity with the provided information. Validates the input before persisting."
                });

            group.MapGet("/{applicationId}", FlowEndpoints.GetFlow)
                .WithName("flowByApplicationId")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Retrieve an application flow by the application ID",
                    Description = "Fetches the flow details associated with the specified unique application identifier."
                });
        }
    }
}
