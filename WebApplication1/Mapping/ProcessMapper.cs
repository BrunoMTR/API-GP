using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Mapping
{
    public class ProcessMapper : IProcessMapper
    {
        // DTO → Model
        public Process Map(ProcessDto process)
        {
            if (process == null) return null;

            return new Process
            {
                ApplicationId = process.ApplicationId,
                CreatedBy = process.CreatedBy,
                Note = process.Note,
            };
        }

        // Model → DTO
        public ProcessDto Map(Process process)
        {
            if (process == null) return null;

            return new ProcessDto
            {
                ApplicationId = process.ApplicationId,
                CreatedBy = process.CreatedBy,
                Note = process.Note,
            };
        }
    }
}
