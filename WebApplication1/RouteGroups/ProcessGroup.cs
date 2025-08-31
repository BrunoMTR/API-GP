using Asp.Versioning;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;

namespace Presentation.RouteGroups
{
    public static class ProcessGroup
    {
        public static void AddProcessEndpoints(this WebApplication app)
        {
            var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();
            var group = app.MapGroup("processes").WithTags("Process").WithApiVersionSet(versionSet).HasApiVersion(1.0);

            group.MapPost("/", ProcessEndpoints.PostProcess)
                 .AddEndpointFilter<InputValidatorFilter<Process>>()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create a new Process",
                    Description = "Creates a new process entity with the provided information. Validates the input before persisting."
                });

            group.MapGet("/{processId}", ProcessEndpoints.GetProcess)
                .WithName("processId")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Retrieve an process by ID",
                     Description = "Fetches the process details associated with the specified unique identifier."
                 });

            group.MapPatch("/{processId}", ProcessEndpoints.PostProcessAprove)
                .WithName("processAprove")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Approve a process by ID",
                     Description = "Approves the process associated with the specified unique identifier, advancing its state in the workflow."
                 });


            group.MapGet("/application/{applicationId}", ProcessEndpoints.GetProcessByApplicationId)
                .WithName("processesByApplicationId")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Retrieve all processes by Application ID",
                     Description = "Fetches all processes details associated with the specified unique aplication identifier."
                 });


        }
    }
}
