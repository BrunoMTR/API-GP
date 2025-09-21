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
                Status = process.Status,
                At = process.At,

            };

            await _demoContext.AddAsync(processEntity);
            await _demoContext.SaveChangesAsync();
            process.Id = processEntity.Id;
            return process;
        }





        public async Task<List<ProcessFlowDto>> GetAllAsync(ProcessQueryParams query)
        {
            var processesQuery = _demoContext.Process
                .Include(p => p.Histories)
                .Include(p => p.Application)
                .Include(p => p.Unit)
                .AsQueryable();

            // 🔎 Filtros
            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                processesQuery = processesQuery.Where(p =>
                    p.Id.ToString().ToLower().Contains(search) ||
                    p.CreatedBy.ToLower().Contains(search) ||
                    p.Unit.Name.ToLower().Contains(search) ||
                    p.Application.Name.ToLower().Contains(search));
            }

            if (!string.IsNullOrEmpty(query.Application) && query.Application != "all")
            {
                processesQuery = processesQuery.Where(p => p.Application.Name == query.Application);
            }

            if (!string.IsNullOrEmpty(query.DateFilter) && query.DateFilter != "all")
            {
                var now = DateTime.UtcNow;
                if (query.DateFilter == "last7")
                {
                    processesQuery = processesQuery.Where(p => p.CreatedAt >= now.AddDays(-7));
                }
                else if (query.DateFilter == "last30")
                {
                    processesQuery = processesQuery.Where(p => p.CreatedAt >= now.AddDays(-30));
                }
            }

            // 🔢 Total de processos antes da paginação
            var totalCount = await processesQuery.CountAsync();

            // 📄 Paginação
            processesQuery = processesQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize);

            var processes = await processesQuery.ToListAsync();

            var results = new List<ProcessFlowDto>();
            foreach (var p in processes)
            {
                var flow = await new FlowRepository(_demoContext)
                    .GetFlowByApplicationId(p.ApplicationId);

                if (flow == null) continue;

                var visitedAtValues = p.Histories
                    .OrderBy(h => h.UpdatedAt)
                    .Select(h => h.At)
                    .ToHashSet();

                var currentAt = p.At;

                var nodes = flow.Nodes.Select(n =>
                {
                    int nodeId = int.Parse(n.Id.Substring(1));
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

                var application = await _demoContext.Application
                    .FirstOrDefaultAsync(a => a.Id == p.ApplicationId);

                var unit = await _demoContext.Unit
                    .FirstOrDefaultAsync(u => u.Id == p.At);

                results.Add(new ProcessFlowDto
                {
                    Id = p.Id,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy,
                    Unit = new UnitDto
                    {
                        Id = unit.Id,
                        Name = unit.Name,
                        Abbreviation = unit.Abbreviation,
                        Email = unit.Email
                    },
                    Application = new ApplicationDto
                    {
                        Id = application.Id,
                        Name = application.Name,
                        Abbreviation = application.Abbreviation,
                        Team = application.Team,
                        TeamEmail = application.TeamEmail,
                        ApplicationEmail = application.ApplicationEmail
                    },
                    Status = p.Status,
                    Nodes = nodes,
                    Edges = flow.Edges,
                    ProcessCount = totalCount
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
