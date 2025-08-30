using Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Forms;



namespace Presentation.Endpoints
{
    public class ApplicationEndpoints
    {
        public static async Task<IResult> PostApplication([FromBody] CreateApplicationFlowRequest request,
            [FromServices] IApplicationMapper mapperApplication, [FromServices] IGraphMapper mapperGraph,
            [FromServices] IApplicationService applicationService)
        {
            var applicationDto = mapperApplication.Map(request.Application);
            var graphDto = mapperGraph.Map(request.Graph);
            var newApplication = await applicationService.Create(applicationDto,graphDto);
            if (newApplication is null)
                return Results.Conflict("Já existe outra aplicação com o mesmo nome ou Abreviatura.");

            return Results.CreatedAtRoute(
                "applicationId",
                new
                { id = newApplication.Application.Id },
                newApplication
                );
        }

        public static async Task<IResult> GetApplication(int id,
        [FromServices] IApplicationService applicationService)
        {
            var application = await applicationService.Retrieve(id);

            if (application == null)
                return Results.NotFound();
            
            return Results.Ok(application);
        }



        public static async Task<IResult> GetAllApplications(
           [FromServices] IApplicationService applicationService)
        {
            var applications = await applicationService.GetAll();

            return Results.Ok(applications);
        }

        public static async Task<IResult> UpdateApplication(
            [FromBody] Application application,
            [FromRoute] int id,
            [FromServices] IApplicationService applicationService,
            [FromServices] IApplicationMapper mapper)
        {
            var applicationDto = mapper.Map(application);
            var updated = await applicationService.Update(applicationDto, id);

            if (updated is null)
                return Results.NotFound();

            if (string.IsNullOrWhiteSpace(updated.Name))
                return Results.Conflict("Já existe outra aplicação com o mesmo nome ou Abreviatura.");

            return Results.Ok(mapper.Map(updated));
        }


        public static async Task<IResult> DeleteApplication(int id,
            [FromServices] IApplicationService applicationService)
        {
            var existing = await applicationService.Retrieve(id);
            if (existing == null)
                return Results.NotFound();

            await applicationService.Delete(id);
            return Results.NoContent();
        }




    }
}
