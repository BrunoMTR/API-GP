using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProcessRepository
    {
        Task<ProcessDto> RetrieveAsync(int id);
        Task<List<ProcessDto>> GetAllAsync();
        Task<List<ProcessDto>> GetAllByApplicationIdAsync(int applicationId);
        Task<ProcessDto> CreateAsync(ProcessDto process);
        Task<ProcessDto> UpdateAsync(int processId, ProcessDto process);

    }
}
