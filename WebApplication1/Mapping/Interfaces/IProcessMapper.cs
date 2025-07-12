using Domain.DTOs;
using Presentation.Models;
using Presentation.Models.Form;

namespace Presentation.Mapping.Interfaces
{
    public interface IProcessMapper
    {
        public ProcessDto? Map(Process process);
        public Process Map(ProcessDto process);
        public List<Process> Map(List<ProcessDto> processes);
        public ProcessDto Map(ProcessForm process);
    }
}
