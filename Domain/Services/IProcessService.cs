using Domain.DTOs;
using Domain.DTOs.FlowDTOs;

namespace Domain.Services
{
    public interface IProcessService
    {
        Task<ProcessDto> Retrieve(int id);
        Task<List<ProcessFlowDto>> GetAll();
        Task<List<ProcessDto>> GetAllByApplicationId(int applicationId);
        Task<ProcessDto> Create(ProcessDto process);

        Task<ProcessDto> Approve(int processId, string updatedBy);
        Task<ProcessFlowDto?> GetProcessFlow(int processId);


    }
}
