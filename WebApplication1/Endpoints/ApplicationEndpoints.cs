using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Requests;


namespace Presentation.Endpoints
{
    public class ApplicationEndpoints
    {
        public static async Task<IResult> PostApplication([FromBody] CreateApplication application,
            [FromServices] IApplicationMapper mapper,
            [FromServices] IApplicationService applicationService)
        {
            var applicationDto = mapper.Map(application);
            var newApplication = await applicationService.Create(applicationDto);
            return Results.CreatedAtRoute(
                "applicationId",
                new
                { id = newApplication.Id },
                newApplication
                );
        }

        public static async Task<IResult> GetApplication(int id,
        [FromServices] IApplicationService applicationService,
        [FromServices] IApplicationMapper mapper)
        {
            var application = await applicationService.Retrieve(id);

            if (application == null)
                return Results.NotFound();

            var applicationModel = mapper.Map(application);
            return Results.Ok(applicationModel);
        }



        public static async Task<IResult> GetAllApplications(
           [FromServices] IApplicationService applicationService,
           [FromServices] IApplicationMapper mapper)
        {
            var applications = await applicationService.GetAll();
            var applicationDtos = applications.Select(mapper.Map);
            return Results.Ok(applicationDtos);
        }

        public static async Task<IResult> UpdateApplication([FromBody] UpdateApplication application,
            [FromRoute] int id,
            [FromServices] IApplicationService applicationService,
            [FromServices] IApplicationMapper mapper)
        {
            var existing = await applicationService.Retrieve(id);
            if (existing == null)
                return Results.NotFound();

            var applicationDto = mapper.Map(application);
            var updated = await applicationService.Update(applicationDto,id);
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
