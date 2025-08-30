using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Forms;

namespace Presentation.Endpoints
{
    public class ProcessEndpoints
    {
        public static async Task<IResult> PostProcess([FromBody] Process process,
           [FromServices] IProcessMapper mapper,
           [FromServices] IProcessService processService)
        {
          var processDto = mapper.Map(process);
          var newProcess = await processService.Create(processDto);

            return Results.CreatedAtRoute(
                "processId",
                new
                { processId = newProcess.Id },
                newProcess
            );
        }

        public static async Task<IResult> GetProcess(int processId,
           [FromServices] IProcessService processService)
        {
            var process = await processService.Retrieve(processId);
            if (process is null)
                return Results.NotFound();

            return Results.Ok(process);
        }

        public static async Task<IResult> PostProcessAprove([FromRoute] int processId,
           [FromServices] IProcessService processService)
        {
            var updatedProcess = await processService.Approve(processId);

            return Results.Ok(updatedProcess);
        }

    }
}
