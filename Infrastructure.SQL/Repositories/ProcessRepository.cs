using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.DTOs.FlowDTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories
{
    public class ProcessRepository : IProcessRepository
    {

        private readonly DemoContext _demoContext;
        public ProcessRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }


        public async Task<ProcessDto> CreateAsync(ProcessDto process)
        {
            var processEntity = new ProcessEntity
            {
                Id = process.Id,
                ApplicationId = process.ApplicationId,
                CreatedAt = process.CreatedAt,
                CreatedBy = process.CreatedBy,
                Approvals = process.Approvals,
                Status =  ProcessStatus.Initiated,
                At = process.At
            };

            await _demoContext.AddAsync(processEntity);
            await _demoContext.SaveChangesAsync();
            process.Id = processEntity.Id;
            return process;
        }

        public async Task<List<ProcessFlowDto>> GetAllAsync()
        {
            var processes = await _demoContext.Process
                .Include(p => p.Histories)
                .ToListAsync();

            var results = new List<ProcessFlowDto>();

            foreach (var p in processes)
            {
                // 1. Buscar o fluxo da aplicação associada
                var flow = await new FlowRepository(_demoContext)
                    .GetFlowByApplicationId(p.ApplicationId);

                if (flow == null) continue;

                // 2. Calcular nodes visitados
                var visitedAtValues = p.Histories
                    .OrderBy(h => h.UpdatedAt)
                    .Select(h => h.At)
                    .ToHashSet();

                var currentAt = p.At;

                // 3. Marcar os nodes
                var nodes = flow.Nodes.Select(n =>
                {
                    int nodeId = int.Parse(n.Id.Substring(1)); // n1 -> 1
                    var status = visitedAtValues.Contains(nodeId) ? "visited" : "pending";
                    if (nodeId == currentAt) status = "current";

                    return new ReactFlowNodeDto
                    {
                        Id = n.Id,
                        Position = n.Position,
                        Type = n.Type,
                        Data = new NodeDataDto
                        {
                            Label = n.Data.Label,
                            Status = status
                        }
                    };
                }).ToList();

                // 4. Edges iguais ao fluxo original
                var edges = flow.Edges;

                // 5. Adicionar metadados do processo
                results.Add(new ProcessFlowDto
                {
                    Id = p.Id.ToString(),
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy,
                    At = p.At.ToString(),
                    Workflows = p.ApplicationId.ToString(), // podes mudar se tiveres tabela Workflow
                    Status = p.Status.ToString(),
                    Nodes = nodes,
                    Edges = edges
                });
            }

            return results;
        }




        public async Task<List<ProcessDto>> GetAllByApplicationIdAsync(int applicationId)
        {
            var processes = await _demoContext.Process
                .Where(p => p.ApplicationId == applicationId)
                .Include(p => p.Histories) 
                .ToListAsync();

            return processes.Select(p => new ProcessDto
            {
                Id = p.Id,
                ApplicationId = p.ApplicationId,
                At = p.At,
                Approvals = p.Approvals,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                Histories = p.Histories
                    .Select(h => new HistoryDto
                    {
                        Id = h.Id,
                        ApplicationId = h.ApplicationId,
                        ProcessId = h.ProcessId,
                        At = h.At,
                        UpdatedBy = h.UpdatedBy,
                        UpdatedAt = h.UpdatedAt
                    }).ToList()
            }).ToList();
        }


        public async Task<ProcessDto> RetrieveAsync(int id)
        {
            return await _demoContext.Process
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ProcessDto
                {
                    ApplicationId = x.ApplicationId,
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    At = x.At,
                    Approvals = x.Approvals,
                    Status = x.Status,
                    Histories = x.Histories
                        .Select(h => new HistoryDto
                        {
                            Id = h.Id,
                            ApplicationId = h.ApplicationId,
                            ProcessId = h.ProcessId,
                            At = h.At,
                            UpdatedBy = h.UpdatedBy,
                            UpdatedAt = h.UpdatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }



        public async Task<ProcessDto> UpdateAsync(int processId, ProcessDto process)
        {
            var entity = await _demoContext.Process
                .Include(p => p.Histories) // garante que o EF carregue o histórico
                .FirstOrDefaultAsync(p => p.Id == processId);

            if (entity == null)
                throw new Exception("Process not found.");

            bool hasChanges = entity.At != process.At ||
                              entity.Approvals != process.Approvals ||
                              entity.Status != process.Status;

            if (!hasChanges)
                return process;

            entity.At = process.At;
            entity.Approvals = process.Approvals;
            entity.Status = process.Status;

            _demoContext.Process.Update(entity);
            await _demoContext.SaveChangesAsync();

            process.Id = entity.Id;
            process.Status = entity.Status;

            // Mapeia corretamente para DTO
            process.Histories = entity.Histories?
                .Select(h => new HistoryDto
                {
                    Id = h.Id,
                    ApplicationId = h.ApplicationId,
                    ProcessId = h.ProcessId,
                    At = h.At,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedAt = h.UpdatedAt
                }).ToList();

            return process;
        }



        public async Task<ProcessFlowDto?> GetProcessFlow(int processId, ReactFlowDto flow)
        {

            if (flow == null) return null;

            if (flow == null) return null;

            // 2. Pega o processo e histórico
            var process = await RetrieveAsync(processId);
            if (process == null) return null;

            var visitedAtValues = process.Histories
                .OrderBy(h => h.UpdatedAt)
                .Select(h => h.At)
                .ToHashSet();

            var currentAt = process.At;

            // 3. Marca nodes como "percorrido", "atual" ou normal
            var nodes = flow.Nodes.Select(n =>
            {
                int nodeId = int.Parse(n.Id.Substring(1)); // n1 -> 1
                var status = visitedAtValues.Contains(nodeId) ? "visited" : "pending";
                if (nodeId == currentAt) status = "current";

                return new ReactFlowNodeDto
                {
                    Id = n.Id,
                    Position = n.Position,
                    Type = n.Type,
                    Data = new NodeDataDto
                    {
                        Label = n.Data.Label,
                        Status = status // podemos usar no front pra cores
                    }
                };
            }).ToList();

            // 4. Mantém edges do fluxo original
            var edges = flow.Edges;

            return new ProcessFlowDto
            {
                Nodes = nodes,
                Edges = edges
            };
        }






    }
}
