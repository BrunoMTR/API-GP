using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.DTOs.FlowDTOs;
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
        Task<List<ProcessFlowDto>> GetAllAsync(ProcessQueryParams query);
        Task<List<ProcessDto>> GetAllByApplicationIdAsync(int applicationId);
        Task<ProcessDto> CreateAsync(ProcessDto process);
        Task<ProcessDto> UpdateAsync(int processId, ProcessDto process);

        Task<ProcessFlowDto?> GetProcessFlow(int processId, ReactFlowDto flow);

    }
}
