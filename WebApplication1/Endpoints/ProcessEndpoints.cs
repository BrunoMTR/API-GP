using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Forms;
using Presentation.Models.Request;

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
            var process = await processService.GetProcessFlow(processId);
            if (process is null)
                return Results.NotFound();

            return Results.Ok(process);
        }

        public static async Task<IResult> GetAllProcesses([FromServices] IProcessService processService)
        {
            var processes = await processService.GetAll();
            if (processes is null)
                return Results.NotFound();

            return Results.Ok(processes);
        }

        public static async Task<IResult> PostProcessAprove([FromBody] ApproveProcessRequest request,
           [FromServices] IProcessService processService)
        {
            var updatedProcess = await processService.Approve(request.ProcessId, request.UpdatedBy);

            return Results.Ok(updatedProcess);
        }

        public static async Task<IResult> GetProcessByApplicationId(int applicationId,
          [FromServices] IProcessService processService)
        {
            var processes = await processService.GetAllByApplicationId(applicationId);
            if (processes is null)
                return Results.NotFound();

            return Results.Ok(processes);
        }

    }
}
