using Asp.Versioning;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;
using Presentation.Models.Forms;


namespace Presentation.RouteGroups
{
    public static class ApplicationGroup
    {
        public static void AddApllicatioGroup(this WebApplication app)
        {

            var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();
            var group = app.MapGroup("applications").WithTags("Applications").WithApiVersionSet(versionSet).HasApiVersion(1.0);

            group.MapPost("/", ApplicationEndpoints.PostApplication)
                 .RequireAuthorization()
                .AddEndpointFilter<InputValidatorFilter<CreateApplicationFlowRequest>>()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create a new application and its flow",
                    Description = "Creates a new application entity with the provided information. Validates the input before persisting."
                });

            group.MapGet("/{id}", ApplicationEndpoints.GetApplication)
                .RequireAuthorization()
                .WithName("applicationId")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Retrieve an application by ID",
                    Description = "Fetches the application details associated with the specified unique identifier."
                });

            group.MapGet("/", ApplicationEndpoints.GetAllApplications)
                 .RequireAuthorization()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "List all applications",
                    Description = "Returns a list of all applications registered in the system."
                });

            group.MapPut("/{id}", ApplicationEndpoints.UpdateApplication)
                .RequireAuthorization()
                .AddEndpointFilter<InputValidatorFiltersUpdate<Application>>()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update an existing application",
                    Description = "Updates the properties of an existing application using the specified ID and request body."
                });

            group.MapDelete("/{id}", ApplicationEndpoints.DeleteApplication)
                .RequireAuthorization()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Delete an application",
                    Description = "Deletes the application that matches the provided ID from the system."
                });

        }
    }
}
