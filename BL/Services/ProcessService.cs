using BL.Handlers;
using BL.Utils;
using Domain.DTOs;
using Domain.DTOs.FlowDTOs;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Infrastructure.SQL.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading;


namespace BL.Services
{
    public class ProcessService : IProcessService
    {
        private IProcessRepository _processRepository;
        private IFlowRepository _flowRepository;
        private IHistoryRepository _historyRepository;
        private IApplicationRepository _applicationRepository;
        private readonly DemoContext _demoContext;
        private readonly DocumentationHandler _documentationHandler;

        public ProcessService(IProcessRepository processRepository,
            IFlowRepository flowRepository,
            IHistoryRepository historyRepository,
            IApplicationRepository applicationRepository,
            DocumentationHandler documentationHandler,
            DemoContext demoContext)
        {
            _processRepository = processRepository;
            _flowRepository = flowRepository;
            _historyRepository = historyRepository;
            _applicationRepository = applicationRepository;
            _demoContext = demoContext;
            _documentationHandler = documentationHandler;
        }


        public async Task<Response<ProcessDto>> ApproveAsync(
        int processId,
        string updatedBy,
        string note,
        IFormFile? file,
        CancellationToken cancellationToken)
        {
            var process = await _processRepository.RetrieveAsync(processId);

            if (process == null)
                return Response<ProcessDto>.Fail("Process not found.");

            if (process.Status == ProcessStatus.Concluded)
                return Response<ProcessDto>.Fail("Process already concluded.");

            if (process.Status == ProcessStatus.Uploading)
                return Response<ProcessDto>.Fail("Uploading documentation linked to the specified process.");

            if (process.Status == ProcessStatus.Canceled)
                return Response<ProcessDto>.Fail("Process already canceled.");

            var flow = await _flowRepository.GetByApplicationAsync(process.ApplicationId);
            var nodes = flow?.Nodes;

            if (flow == null || nodes == null)
                return Response<ProcessDto>.Fail("No flow found for the application associated with this process.");

            var currentNode = GraphUtil.GetCurrentNode(nodes, process.At);
            if (currentNode == null)
                return Response<ProcessDto>.Fail("Current node not found in flow or no forward node available.");

            var initialNode = GraphUtil.GetInitialNode(nodes);
            if (initialNode == null)
                return Response<ProcessDto>.Fail("Initial node not found.");

            int totalRequiredApprovals = GraphUtil.GetTotalRequiredApprovals(nodes);
            bool willTransition = GraphUtil.HasReachedApprovalQuorum(currentNode, process.Approvals);
            int nextNodeId = GraphUtil.GetNextNodeId(currentNode);

            var strategy = _demoContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _demoContext.Database.BeginTransactionAsync();
                try
                {
                    // Grava histórico com a nota atual antes de sobrescrever
                    var historyDto = new HistoryDto
                    {
                        ApplicationId = process.ApplicationId,
                        ProcessId = process.Id,
                        At = process.At,
                        UpdatedBy = updatedBy,
                        UpdatedAt = DateTime.UtcNow,
                        Note = process.Note // pega o valor atual antes da alteração
                    };
                    await _historyRepository.CreateAsync(historyDto);

                    // Atualiza o processo
                    process.Note = note; // novo note enviado pelo utilizador

                    if (willTransition)
                    {
                        process.Approvals = 0;
                        process.At = nextNodeId;

                        if (nextNodeId == initialNode.OriginId)
                        {
                            process.Status = ProcessStatus.Concluded;
                            process.Approvals = totalRequiredApprovals;
                        }
                        else if (process.Status == ProcessStatus.Initiated)
                        {
                            process.Status = ProcessStatus.Pending;
                        }
                    }
                    else
                    {
                        process.Approvals += 1;
                    }

                    // Se houver ficheiro, marca como Uploading
                    if (file != null)
                        process.Status = ProcessStatus.Uploading;

                    var result = await _processRepository.UpdateAsync(processId, process);

                    await transaction.CommitAsync();

                    // Envio para background handler (após commit)
                    if (file != null)
                    {
                        var application = await _applicationRepository.RetrieveAsync(process.ApplicationId);
                        if (application == null)
                            return Response<ProcessDto>.Fail("Application not found.");

                        var message = new DocumentMessageDto
                        {
                            ProcessId = result.Id,
                            ApplicationName = application.Abbreviation,
                            TempFilePath = string.Empty,
                            UploadedBy = updatedBy,
                            At = process.At
                        };

                        await _documentationHandler.QueueFileAsync(message, file, cancellationToken);
                    }

                    return Response<ProcessDto>.Ok(result, "Process approved successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<ProcessDto>.Fail($"Error while approving process: {ex.Message}");
                }
            });
        }



        public async Task<Response<ProcessDto>> CreateAsync(ProcessDto process,
            IFormFile? file,
            CancellationToken cancellationToken)
        {
            var flow = await _flowRepository.GetByApplicationAsync(process.ApplicationId);

            if (flow == null || flow.Nodes == null || !flow.Nodes.Any())
                return Response<ProcessDto>.Fail("No flow found for the application.");

            var nodes = flow.Nodes;

            var initialNode = GraphUtil.GetInitialNode(nodes);
            if (initialNode == null)
                return Response<ProcessDto>.Fail("Initial node not found.");

            var initialEdge = GraphUtil.GetInitialEdge(nodes, initialNode.OriginId);
            if (initialEdge == null)
                return Response<ProcessDto>.Fail("Initial node has no outgoing 'AVANÇO' edge.");

            var strategy = _demoContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _demoContext.Database.BeginTransactionAsync();
                try
                {
                    if (initialEdge.Approvals == 0)
                    {
                        process.At = initialEdge.DestinationId;
                        process.Status = ProcessStatus.Pending;
                    }
                    else
                    {
                        process.At = initialNode.OriginId;
                        process.Status = ProcessStatus.Initiated;
                    }

                    if (file != null)
                        process.Status = ProcessStatus.Uploading;


                    var result = await _processRepository.CreateAsync(process);

                    if (initialEdge.Approvals == 0)
                    {
                        var historyDto = new HistoryDto
                        {
                            ApplicationId = result.ApplicationId,
                            ProcessId = result.Id,
                            At = initialNode.OriginId,
                            UpdatedBy = process.CreatedBy,
                            UpdatedAt = DateTime.UtcNow,
                            Note = process.Note
                        };
                        await _historyRepository.CreateAsync(historyDto);
                    }

                    await transaction.CommitAsync();

                    if (file != null)
                    {
                        var application = await _applicationRepository.RetrieveAsync(process.ApplicationId);
                        if (application == null)
                            return Response<ProcessDto>.Fail("Application not found.");

                        var message = new DocumentMessageDto
                        {
                            ProcessId = result.Id,
                            ApplicationName = application.Abbreviation,
                            TempFilePath = string.Empty,
                            UploadedBy = process.CreatedBy,
                            At = process.At
                        };

                        await _documentationHandler.QueueFileAsync(message, file, cancellationToken);
                    }

                    return Response<ProcessDto>.Ok(result, "Process created successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<ProcessDto>.Fail($"Error while creating process: {ex.Message}");
                }
            });
        }

        public async Task<Response<List<ProcessFlowDto>>> GetAllAsync(Query query)
        {
            var (processes, totalCount) = await _processRepository.GetAllAsync(query);


            var results = new List<ProcessFlowDto>();

            foreach (var process in processes)
            {
                var normalizedNodes = await _flowRepository.GetByApplicationAsync(process.Application.Id);
                if (normalizedNodes == null)
                    return Response<List<ProcessFlowDto>>.Fail("No flow found for one of the applications.");

                var flow = GraphUtil.MapToFlow(normalizedNodes);
                if (flow == null) continue;

                results.Add(GraphUtil.MapToHistory(process, flow, totalCount));
            }

            return Response<List<ProcessFlowDto>>.Ok(
            results,
            results.Any() ? "Processes retrieved successfully." : "No processes found.",
            totalCount);
        }

        public async Task<Response<List<DocumentationDto>>> GetAllDocumentationsAsync(Query query)
        {
            var (docs, totalCount) = await _processRepository.GetAllDocumentationsAsync(query);

            return Response<List<DocumentationDto>>.Ok(
            docs,
            docs.Any() ? "Docs retrieved successfully." : "No Docs found.",
            totalCount);



        }

        public async Task<Response<ProcessDto>> CancelAsync(int processId, string updatedBy, string note)
        {
            var process = await _processRepository.RetrieveAsync(processId);

            if (process == null)
                return Response<ProcessDto>.Fail("Process not found.");

            if (process.Status == ProcessStatus.Concluded)
                return Response<ProcessDto>.Fail("Process already concluded.");

            if (process.Status == ProcessStatus.Uploading)
                return Response<ProcessDto>.Fail("Uploading documentation linked to the specified process.");

            if (process.Status == ProcessStatus.Canceled)
                return Response<ProcessDto>.Fail("Process already canceled.");

            if (process.CreatedBy != updatedBy.Trim())
                return Response<ProcessDto>.Fail("Only the user that reated the process can cancel");

            process.Status = ProcessStatus.Canceled;
           

            var strategy = _demoContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _demoContext.Database.BeginTransactionAsync();

                try
                {
                    var historyDto = new HistoryDto
                    {
                        ApplicationId = process.ApplicationId,
                        ProcessId = process.Id,
                        At = process.At,
                        UpdatedBy = updatedBy,
                        UpdatedAt = DateTime.UtcNow,
                        Note = process.Note
                    };
                    await _historyRepository.CreateAsync(historyDto);

                    process.Note = note;

                    var result = await _processRepository.UpdateAsync(processId, process);

                    await transaction.CommitAsync();
                    return Response<ProcessDto>.Ok(result, "Process canseled successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<ProcessDto>.Fail($"Error while canseling process: {ex.Message}");
                }
            });


        }

        public async Task<Response<ProcessDto>> ReturnAsync(int processId, string updatedBy, string? note)
        {
            var process = await _processRepository.RetrieveAsync(processId);

            if (process == null)
                return Response<ProcessDto>.Fail("Process not found.");

            if (process.Status == ProcessStatus.Concluded)
                return Response<ProcessDto>.Fail("Process already concluded.");

            if (process.Status == ProcessStatus.Uploading)
                return Response<ProcessDto>.Fail("Uploading documentation linked to the specified process.");

            if (process.Status == ProcessStatus.Canceled)
                return Response<ProcessDto>.Fail("Process already canceled.");

            var flow = await _flowRepository.GetByApplicationAsync(process.ApplicationId);
            var nodes = flow?.Nodes;

            if (flow == null || nodes == null)
                return Response<ProcessDto>.Fail("No flow found for the application associated with this process.");

            var currentNode = GraphUtil.GetCurrentNodeRecuo(nodes, process.At);
            if (currentNode == null)
                return Response<ProcessDto>.Fail("Current node not found in flow or no forward node available.");

            var initialNode = GraphUtil.GetInitialNode(nodes);
            if (initialNode == null)
                return Response<ProcessDto>.Fail("Initial node not found.");

            if (initialNode == currentNode)
                return Response<ProcessDto>.Fail("The process cant be return becouse is in the initial stage");

            var previsNode = GraphUtil.GetPreviousNodeId(currentNode);

            process.At = previsNode;
            process.Approvals = 0;

            var strategy = _demoContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _demoContext.Database.BeginTransactionAsync();
                try
                {
                    // Grava histórico com a nota atual antes de sobrescrever
                    var historyDto = new HistoryDto
                    {
                        ApplicationId = process.ApplicationId,
                        ProcessId = process.Id,
                        At = process.At,
                        UpdatedBy = updatedBy,
                        UpdatedAt = DateTime.UtcNow,
                        Note = process.Note // pega o valor atual antes da alteração
                    };
                    await _historyRepository.CreateAsync(historyDto);

                    
                    process.Note = note; 
                   
                    var result = await _processRepository.UpdateAsync(processId, process);

                    await transaction.CommitAsync();
                  

                    return Response<ProcessDto>.Ok(result, "Process return successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<ProcessDto>.Fail($"Error while returning process: {ex.Message}");
                }
            });

        }

      
    }
}
