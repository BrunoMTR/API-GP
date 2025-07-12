using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Repositories
{
    public class ProcessRepositoty : IProcessRepository
    {
        private readonly DemoContext _demoContext;
        public ProcessRepositoty (DemoContext demoContext)
        {
            _demoContext = demoContext;
        }
        public Task<bool> AddDocumentsAsync(int id, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(ProcessDto dto)
        {

            var processEntity = new ProcessEntity
            {
                ProcessCode = dto.ProcessCode,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = dto.LastUpdatedAt,
                Notes = dto.Notes,
                StateId = dto.StateId,
                CreatedById = dto.CreatedById,
                ApplicationId = dto.ApplicationId,
                HolderId = dto.HolderId
            };

            await _demoContext.AddAsync(processEntity);
            await _demoContext.SaveChangesAsync();
            return processEntity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _demoContext.Process
                 .Where(x => x.Id == id)
                 .ExecuteDeleteAsync();
        }

        public async Task<List<ProcessDto>> GetAllAsync()
        {
            return await _demoContext.Process
                .AsNoTracking()
                .Select(x => new ProcessDto
                {
                    Id = x.Id,
                    ProcessCode = x.ProcessCode,
                    CreatedAt = x.CreatedAt,
                    LastUpdatedAt = x.LastUpdatedAt,
                    Notes = x.Notes,
                    StateId = x.StateId,
                    CreatedById = x.CreatedById,
                    ApplicationId = x.ApplicationId,
                    HolderId = x.HolderId
                }).ToListAsync();
        }

        public Task<bool> ReplaceDocumentAsync(int processId, int documentId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<ProcessDto> RetrieveAsync(int id)
        {
            return await _demoContext.Process.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ProcessDto
                {
                    Id = x.Id,
                    ProcessCode = x.ProcessCode,
                    CreatedAt = x.CreatedAt,
                    LastUpdatedAt = x.LastUpdatedAt,
                    Notes = x.Notes,
                    StateId = x.StateId,
                    CreatedById = x.CreatedById,
                    ApplicationId = x.ApplicationId,
                    HolderId = x.HolderId,

                    State = new StateDto
                    {
                        Id = x.State.Id,
                        Name = x.State.Name,
                        Description = x.State.Description
                    },

                    CreatedBy = new AssociateDto
                    {
                        Id = x.CreatedBy.Id,
                        Name = x.CreatedBy.Name,
                        Email = x.CreatedBy.Email,
                        Code = x.CreatedBy.Code,
                        Role = x.CreatedBy.Role
                    },

                    Application = new ApplicationDto
                    {
                        Id = x.Application.Id,
                        Name = x.Application.Name,
                        Acronym = x.Application.Acronym,
                        Team = x.Application.Team,
                        Email = x.Application.Email
                    },

                    Holder = new HolderDto
                    {
                        Id = x.Holder.Id,
                        Name = x.Holder.Name,
                        Acronym = x.Holder.Acronym,
                        Email = x.Holder.Email
                    },
                    Documents = x.Documents.Select(d => new DocumentDto {
                         Id = d.Id,
                         Name = d.Name,
                         Location = d.Location
                     }).ToList()


                }).FirstOrDefaultAsync();
        }

        public Task<bool> UpdateHolderAsync(int id, int holderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStateAsync(int id, int stateId)
        {
            throw new NotImplementedException();
        }
    }
}
