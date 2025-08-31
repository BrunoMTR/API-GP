using Domain.DTOs;

namespace Domain.Services
{
    public interface IProcessService
    {
        Task<ProcessDto> Retrieve(int id);
        Task<List<ProcessDto>> GetAll();
        Task<List<ProcessDto>> GetAllByApplicationId(int applicationId);
        Task<ProcessDto> Create(ProcessDto process);

        public Task<ProcessDto> Approve(int processId, string updatedBy);
    }
}
