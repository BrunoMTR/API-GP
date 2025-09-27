using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Forms;



namespace Presentation.Endpoints
{
    public class ApplicationEndpoints
    {
        public static async Task<IResult> PostApplication(
        [FromBody] CreateApplicationFlowRequest request,
        [FromServices] IApplicationMapper mapperApplication,
        [FromServices] IGraphMapper mapperGraph,
        [FromServices] IApplicationService applicationService)
        {
            var applicationDto = mapperApplication.Map(request.Application);
            var graphDto = mapperGraph.Map(request.Graph);

            var response = await applicationService.CreateAsync(applicationDto, graphDto);

            if (!response.Success)
                return Results.BadRequest(response.Message);

            return Results.CreatedAtRoute(
                "applicationId",
                new { id = response.Data.Id },
                response
            );
        }

        public static async Task<IResult> GetApplication(int id,
        [FromServices] IApplicationService applicationService)
        {
            var application = await applicationService.RetrieveAsync(id);

            if (application == null)
                return Results.NotFound();
            
            return Results.Ok(application);
        }

        public static async Task<IResult> GetAllApplications(
           [FromServices] IApplicationService applicationService)
        {
            var applications = await applicationService.GetAllAsync();

            return Results.Ok(applications);
        }

        public static async Task<IResult> UpdateApplication(
            [FromBody] Application application,
            [FromRoute] int id,
            [FromServices] IApplicationService applicationService,
            [FromServices] IApplicationMapper mapper)
        {
            var applicationDto = mapper.Map(application);
            var updated = await applicationService.UpdateAsync(applicationDto, id);

            if (updated is null)
                return Results.NotFound();

            if (string.IsNullOrWhiteSpace(updated.Data.Name))
                return Results.Conflict("Já existe outra aplicação com o mesmo nome ou Abreviatura.");

            return Results.Ok(mapper.Map(updated.Data));
        }

        public static async Task<IResult> DeleteApplication(int id,
            [FromServices] IApplicationService applicationService)
        {
            var existing = await applicationService.RetrieveAsync(id);
            if (existing == null)
                return Results.NotFound();

            await applicationService.DeleteAsync(id);
            return Results.NoContent();
        }

    }
}
