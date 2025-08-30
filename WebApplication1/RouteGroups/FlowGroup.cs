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
