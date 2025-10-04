using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.DTOs.FlowDTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

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
                Note = process.Note

            };

            await _demoContext.AddAsync(processEntity);
            await _demoContext.SaveChangesAsync();

            // Recarrega a entidade com os relacionamentos necessários
            var entity = await _demoContext.Process
                .Include(p => p.Histories)
                .Include(p => p.Unit)
                .Include(p => p.Application)
                .FirstOrDefaultAsync(p => p.Id == processEntity.Id);

            if (entity == null)
                throw new Exception("Process not found after creation.");

            // Mapeia para o DTO sem documentations
            var result = new ProcessDto
            {
                Id = entity.Id,
                ApplicationId = entity.ApplicationId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                At = entity.At,
                Approvals = entity.Approvals,
                Status = entity.Status,
                Note = entity.Note,
                Histories = entity.Histories?.Select(h => new HistoryDto
                {
                    Id = h.Id,
                    ApplicationId = h.ApplicationId,
                    ProcessId = h.ProcessId,
                    At = h.At,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedAt = h.UpdatedAt,
                    Notified = h.Notified,
                    Note = h.Note
                }).ToList(),
                Unit = entity.Unit != null ? new UnitDto
                {
                    Id = entity.Unit.Id,
                    Name = entity.Unit.Name,
                    Email = entity.Unit.Email,
                    Abbreviation = entity.Unit.Abbreviation
                } : null,
                Application = entity.Application != null ? new ApplicationDto
                {
                    Id = entity.Application.Id,
                    Name = entity.Application.Name,
                    Abbreviation = entity.Application.Abbreviation,
                    Team = entity.Application.Team,
                    ApplicationEmail = entity.Application.ApplicationEmail,
                    TeamEmail = entity.Application.TeamEmail
                } : null
            };

            return result;
        }

        public async Task<(List<ProcessDto> Processes, int TotalCount)> GetAllAsync(Query query)
        {
            var processesQuery = _demoContext.Process
                .Include(p => p.Histories)
                .Include(p => p.Application)
                .Include(p => p.Unit)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                processesQuery = processesQuery.Where(p =>
                    p.Id.ToString().Contains(search) ||
                    (p.CreatedBy != null && p.CreatedBy.ToLower().Contains(search)) ||
                    (p.Unit != null && p.Unit.Name != null && p.Unit.Name.ToLower().Contains(search)) ||
                    (p.Application != null && p.Application.Name != null && p.Application.Name.ToLower().Contains(search))
                );
            }

            if (query.ApplicationId.HasValue && query.ApplicationId.Value > 0)
            {
                processesQuery = processesQuery.Where(p => p.ApplicationId == query.ApplicationId.Value);
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

            var totalCount = await processesQuery.CountAsync();

            processesQuery = processesQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize);

            var processes = await processesQuery.ToListAsync();
            var results = processes.Select(p => new ProcessDto
            {
                Id = p.Id,
                ApplicationId = p.ApplicationId,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                At = p.At,
                Approvals = p.Approvals,
                Status = p.Status,
                Histories = p.Histories?
                    .Select(h => new HistoryDto
                    {
                        Id = h.Id,
                        ApplicationId = h.ApplicationId,
                        ProcessId = h.ProcessId,
                        At = h.At,
                        UpdatedBy = h.UpdatedBy,
                        UpdatedAt = h.UpdatedAt
                    }).ToList(),
                Application = p.Application != null ? new ApplicationDto
                {
                    Id = p.Application.Id,
                    Name = p.Application.Name,
                    Abbreviation = p.Application.Abbreviation,
                    ApplicationEmail = p.Application.ApplicationEmail,
                    Team = p.Application.Team,
                    TeamEmail = p.Application.TeamEmail
                } : null,
                Unit = p.Unit != null ? new UnitDto
                {
                    Id = p.Unit.Id,
                    Name = p.Unit.Name,
                    Abbreviation = p.Unit.Abbreviation,
                    Email = p.Unit.Email
                } : null
            }).ToList();
            return (results, totalCount);
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
                .Include(p => p.Histories)
                .Include(p => p.Documentations)
                .Include(p => p.Unit)
                .Include(p => p.Application)
                .FirstOrDefaultAsync(p => p.Id == processId);

            if (entity == null)
                throw new Exception("Process not found.");

            bool hasChanges = entity.At != process.At ||
                              entity.Approvals != process.Approvals ||
                              entity.Status != process.Status;

            if (hasChanges)
            {
                entity.At = process.At;
                entity.Approvals = process.Approvals;
                entity.Status = process.Status;
                entity.Note = process.Note;

                _demoContext.Process.Update(entity);
                await _demoContext.SaveChangesAsync();
            }

            // Map DTO completo
            var result = new ProcessDto
            {
                Id = entity.Id,
                ApplicationId = entity.ApplicationId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                At = entity.At,
                Approvals = entity.Approvals,
                Status = entity.Status,
                Note = entity.Note,
                Histories = entity.Histories?.Select(h => new HistoryDto
                {
                    Id = h.Id,
                    ApplicationId = h.ApplicationId,
                    ProcessId = h.ProcessId,
                    At = h.At,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedAt = h.UpdatedAt,
                    Notified = h.Notified,
                    Note = h.Note
                }).ToList(),
                Documentations = entity.Documentations?.Select(d => new DocumentationDto
                {
                    Id = d.Id,
                    ProcessId = d.ProcessId,
                    FileName = d.FileName,
                    FilePath = d.FilePath,
                    FileSize = d.FileSize,
                    FileType = d.FileType,
                    UploadedAt = d.UploadedAt,
                    UploadedBy = d.UploadedBy,
                    At = d.At
                }).ToList(),
                Unit = entity.Unit != null ? new UnitDto
                {
                    Id = entity.Unit.Id,
                    Name = entity.Unit.Name,
                    Email = entity.Unit.Email,
                    Abbreviation  = entity.Unit.Abbreviation

                } : null,
                Application = entity.Application != null ? new ApplicationDto
                {
                    Id = entity.Application.Id,
                    Name = entity.Application.Name,
                    Abbreviation = entity.Application.Abbreviation,
                    Team = entity.Application.Team,
                    ApplicationEmail = entity.Application.ApplicationEmail,
                    TeamEmail = entity.Application.TeamEmail
                } : null
            };

            return result;
        }











    }
}
