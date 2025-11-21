using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models.Forms;
using Presentation.Models.Request;
using System.Threading;
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
                Note = process.Note
            };

            var processDto = mapper.Map(processModel);

            var newProcess = await processService.CreateAsync(processDto, process.File, cancellationToken);
            if(!newProcess.Success)
                return Results.BadRequest(newProcess.Message);

            return Results.Created($"/process/{newProcess.Data.Id}", newProcess);
        }

        
        public static async Task<IResult> PatchReturnProcess(
            [FromRoute]    int processId,
            [FromBody]     PatchProcessRequest request,
            [FromServices] IProcessService processService)
        {
            var result = await processService.ReturnAsync(processId,request.UpdatedBy, request.Note);
            if (!result.Success)
                return Results.BadRequest(result.Message);

            return Results.Ok(result);
        }
        
        public static async Task<IResult> PatchCancelProcess(
            [FromRoute] int processId,
            [FromBody] PatchProcessRequest request,
            [FromServices] IProcessService processService)
        {
            var process = await processService.CancelAsync(processId, request.UpdatedBy, request.Note);
            if (!process.Success)
                return Results.BadRequest(process.Message);
            return Results.Ok(process);
        }

        public static async Task<IResult> GetProcesses(
            [FromServices] IProcessService processService,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] int? applicationId,
            [FromQuery] string? dateFilter
)
        {
            var query = new Query
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search,
                ApplicationId = applicationId,
                DateFilter = dateFilter
            };

            var processes = await processService.GetAllAsync(query);
            return Results.Ok(processes);
        }
               
        
        public static async Task<IResult>GetDocs(
            [FromServices] IProcessService processService,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] int? applicationId,
            [FromQuery] string? dateFilter
            )
        {
            var query = new Query
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search,
                ApplicationId = applicationId,
                DateFilter = dateFilter
            };

            var docs = await processService.GetAllDocumentationsAsync(query);
            return Results.Ok(docs);
        }

        public static async Task<IResult> PostAproveProcess([FromForm] AproveProcessRequest request,
           [FromServices] IProcessService processService,
           CancellationToken cancellationToken)
        {
            var process = await processService.ApproveAsync(request.ProcessId, request.UpdatedBy, request.Note, request.File, cancellationToken);
            if(!process.Success)
                return Results.BadRequest(process.Message);

            return Results.Ok(process);
        }

    
        public static async Task<IResult> PostProcessDocumentation(
            [FromForm] DocumentForm document,    
            [FromServices] IDocumentationService service,
            CancellationToken cancellationToken,
            [FromServices] IDocumentMapper mapper
            )
        {
            if(document.File != null)
            {
                await service.UploadAsync(mapper.Map(document), cancellationToken);
                return Results.Created("Uploading document", document.ProcessId);
            }
            return Results.BadRequest("File null");
        }

        public static IResult ConnectHub()
        {
            return Results.Ok();
        }


        

    }

}

