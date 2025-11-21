using Asp.Versioning;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;
using Presentation.Models.Forms;
using DocumentForm = Presentation.Models.Forms.DocumentForm;

namespace Presentation.RouteGroups
{
    public static class ProcessGroup
    {
        public static void AddProcessEndpoints(this WebApplication app)
        {
            var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();
            var group = app.MapGroup("processes").WithTags("Process").WithApiVersionSet(versionSet).HasApiVersion(1.0);

            group.MapPost("/", ProcessEndpoints.PostProcess)
                .AddEndpointFilter<InputValidatorFilter<ProcessForm>>()
                .RequireAuthorization()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create a new Process",
                    Description = "Creates a new process entity with the provided information. Validates the input before persisting."
                });
            group.MapPost("/upload", ProcessEndpoints.PostProcessDocumentation)
                .AddEndpointFilter<InputValidatorFilter<DocumentForm>>()
                
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Upload de file for background processing",
                    Description = "Recive a file and sends for background validation and upload"
                });

            group.MapGet("/", ProcessEndpoints.GetProcesses)
                .RequireAuthorization()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Retrieve all processes flow",
                    Description = "Fetches all processes flow states"
                });

            group.MapGet("/docs", ProcessEndpoints.GetDocs)
              .RequireAuthorization()
              .WithOpenApi(operation => new(operation)
              {
                  Summary = "Retrieve all documentations in lots",
                  Description = "Fetches documents in lots"
              });

            group.MapPatch("/approve", ProcessEndpoints.PostAproveProcess)
                .RequireAuthorization()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                     Summary = "Approve a process by ID",
                     Description = "Approves the process associated with the specified unique identifier, advancing its state in the workflow."
                });

            group.MapPatch("/{processId}/cancel", ProcessEndpoints.PatchCancelProcess)
                .RequireAuthorization()
                .WithName("processCancel")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Cancel a process by ID",
                     Description = "Cancels the process associated with the specified unique identifier, halting its progress in the workflow."
                 });

            group.MapPatch("/{processId}/return", ProcessEndpoints.PatchReturnProcess)
                .RequireAuthorization()
                .WithName("processReturn")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Return a process by ID",
                     Description = "Approves the process associated with the specified unique identifier, returnig its state in the workflow."
                 });




        }
    }
}
