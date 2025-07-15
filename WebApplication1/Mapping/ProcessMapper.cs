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
                CurrentStepId = process.CurrentStepId,
                CreatedById = process.CreatedById,
                ApplicationId = process.ApplicationId,
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
                CurrentStepId = process.CurrentStepId,
                CreatedById = process.CreatedById,
                ApplicationId = process.ApplicationId,
            } : null;
        }

        public List<Process> Map(List<ProcessDto> processes)
        {
            return processes.Select(Map).ToList();
        }

        public ProcessDto Map(ProcessForm process)
        {
            throw new NotImplementedException();
        }

        //public ProcessDto? Map(ProcessForm process)
        //{
        //    return process is not null ? new ProcessDto
        //    {
        //        Id = process.Id,
        //        ProcessCode = process.ProcessCode,
        //        CreatedAt = process.CreatedAt,
        //        LastUpdatedAt = process.LastUpdatedAt,
        //        Notes = process.Notes,
        //        CurrentStepId = process.CurrentStepId,
        //        CreatedById = process.CreatedById,
        //        ApplicationId = process.ApplicationId,

        //        Documents = process.Files?
        //        .Select(f => new DocumentDto
        //        {
        //            File = f,
        //            Name = f.FileName,
        //            Location = string.Empty
        //        }).ToList() ?? new List<DocumentDto>()
        //    } : null;
        //}
    }
}
