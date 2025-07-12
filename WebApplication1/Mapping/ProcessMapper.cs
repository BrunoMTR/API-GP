using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models;
using Presentation.Models.Form;

namespace Presentation.Mapping
{
    public class ProcessMapper : IProcessMapper
    {
        public ProcessDto? Map(Process process)
        {
            return process is not null ? new ProcessDto
            {
                Id = process.Id,
                ProcessCode = process.ProcessCode,
                CreatedAt = process.CreatedAt,
                LastUpdatedAt = process.LastUpdatedAt,
                Notes = process.Notes,
                StateId = process.StateId,
                CreatedById = process.CreatedById,
                ApplicationId = process.ApplicationId,
                HolderId = process.HolderId
            } : null;
        }

        public Process? Map(ProcessDto process)
        {
            return process is not null ? new Process
            {
                Id = process.Id,
                ProcessCode = process.ProcessCode,
                CreatedAt = process.CreatedAt,
                LastUpdatedAt = process.LastUpdatedAt,
                Notes = process.Notes,
                StateId = process.StateId,
                CreatedById = process.CreatedById,
                ApplicationId = process.ApplicationId,
                HolderId = process.HolderId
            } : null;
        }

        public List<Process> Map(List<ProcessDto> processes)
        {
            return processes.Select(Map).ToList();
        }

        public ProcessDto? Map(ProcessForm process)
        {
            return process is not null ? new ProcessDto
            {
                ProcessCode = process.ProcessCode,
                CreatedAt = process.CreatedAt,
                LastUpdatedAt = process.LastUpdatedAt,
                Notes = process.Notes,
                StateId = process.StateId,
                CreatedById = process.CreatedById,
                ApplicationId = process.ApplicationId,
                HolderId = process.HolderId
            } : null;
        }
    }
}
