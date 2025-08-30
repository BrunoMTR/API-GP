using Domain.DTOs;
using Presentation.Models;

namespace Presentation.Mapping.Interfaces
{
    public interface IProcessMapper
    {
        ProcessDto Map(Process process);
        Process Map(ProcessDto process);
    }
}
