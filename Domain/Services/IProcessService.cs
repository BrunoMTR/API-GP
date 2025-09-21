using Domain.DTOs;
using Domain.DTOs.FlowDTOs;
using Microsoft.AspNetCore.Http;

namespace Domain.Services
{
    public interface IProcessService
    {
        Task<List<ProcessFlowDto>> GetAllAsync(ProcessQueryParams query);
        Task<List<ProcessDto>> GetAllByApplicationId(int applicationId);
        Task<ProcessDto> Create(ProcessDto process, bool uploading);
        
        Task<ProcessDto> Approve(int processId, string updatedBy);
        Task<ProcessFlowDto?> GetProcessFlow(int processId);

        Task HandleFileUploadAsync(DocumentMessageDto messag, IFormFile file, CancellationToken cancellationToken);
        Task<ProcessDto> CreateWithFileAsync(ProcessDto process, IFormFile? file, CancellationToken cancellationToken);
        
    }

}