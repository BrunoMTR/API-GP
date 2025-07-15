using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Form;
namespace Presentation.Endpoints
{
    public class ProcessEndpoints
    {
        public static async Task<IResult> PostProcess([FromForm] ProcessForm process,
            [FromServices] IProcessMapper mapper,
            [FromServices] IProcessService processService)
        {
            var processDto = mapper.Map(process);
            return Results.CreatedAtRoute(
                "processById",
                new
                {
                    id = await processService.Create(processDto)
                });
        }
        public static async Task<IResult> GetProcess([FromServices] IProcessService processService,
             [FromServices] IProcessMapper mapper)
        {
            var processes = await processService.GetAll();
            return Results.Ok(mapper.Map(processes));
        }

        public static async Task<IResult>Retrive(int id, 
            [FromServices]IProcessService processService,
            [FromServices]IProcessMapper mapper)
        {
            var process = await processService.Retrieve(id);
            return Results.Ok(mapper.Map(process));
        }
    }
}
