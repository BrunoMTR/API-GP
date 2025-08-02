using Asp.Versioning;
using Presentation.Endpoints;
using Presentation.EndpointsFilters;
using Presentation.Models;
using Presentation.Models.Requests;
using Presentation.Validations;

namespace Presentation.RouteGroups
{
    public static class UnitGroup
    {
        public static void AddUnitEndpoints(this WebApplication app)
        {
            var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();
            var group = app.MapGroup("units").WithTags("Units").WithApiVersionSet(versionSet).HasApiVersion(1.0);

            group.MapPost("/",UnitEndpoints.PostUnit)
                 .AddEndpointFilter<InputValidatorFilter<Unit>>()
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create a new Unit",
                    Description = "Creates a new unit entity with the provided information. Validates the input before persisting."
                });

            group.MapGet("/{unitId}",UnitEndpoints.GetUnit)
                .WithName("unitId")
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Retrieve an unit by ID",
                     Description = "Fetches the unit details associated with the specified unique identifier."
                 });

            group.MapGet("/",UnitEndpoints.GetAllUnits)
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "List all units",
                     Description = "Returns a list of all units registered in the system."
                 });

            group.MapDelete("/{unitId}",UnitEndpoints.DeleteUnit)
                 .WithOpenApi(operation => new(operation)
                 {
                     Summary = "Delete an unit",
                     Description = "Deletes the unit that matches the provided ID from the system."
                 });

            group.MapPut("/{unitId}",UnitEndpoints.UpdateUnit)
                .AddEndpointFilter<InputValidatorFilter<Unit>>()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update an existing unit",
                    Description = "Updates the properties of an existing unit using the specified ID and request body."
                });


        }
    }
}
