using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<ProcessDto>> GetAllAsync()
        {
            var processes = await _demoContext.Process
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







    }
}
