using Domain.Channels;
using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Forms;
using Presentation.Models.Request;
using System.Threading.Channels;
using Process = Presentation.Models.Process;

namespace Presentation.Endpoints
{
    public class ProcessEndpoints
    {
        public static async Task<IResult> PostProcess(
        [FromForm] ProcessForm process,
        [FromServices] IProcessMapper mapper,
        [FromServices] IProcessService processService,
        CancellationToken cancellationToken)
        {
            var processModel = new Process
            {
                ApplicationId = process.ApplicationId,
                CreatedBy = process.CreatedBy,
            };

            var processDto = mapper.Map(processModel);

            var newProcess = await processService.CreateWithFileAsync(processDto, process.File, cancellationToken);

            return Results.Created($"/process/{newProcess.Id}", newProcess);
        }


        public static async Task<IResult> GetAllProcesses(
            [FromServices] IProcessService processService,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] string? application,
            [FromQuery] string? dateFilter
)
        {
            var query = new ProcessQueryParams
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search,
                Application = application,
                DateFilter = dateFilter
            };

            var processes = await processService.GetAllAsync(query);
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

        public static async Task<IResult> UploadFile(
            [FromForm] DocumentForm document,    
            [FromServices] IDocumentationService service,
            CancellationToken cancellationToken,
            [FromServices] IDocumentMapper mapper
            )
        {
            if(document.File != null)
            {
                await service.UploadFile(mapper.Map(document), cancellationToken);
                return Results.Created("Uploading document", document.ProcessId);
            }
            return Results.BadRequest("Dados recebinos incorretos ou em falta");
        }



    }

}

