using Domain.DTOs;
using Domain.DTOs.FlowDTOs;
using Domain.Results;
using Microsoft.AspNetCore.Http;

namespace Domain.Services
{
    public interface IProcessService
    {
        Task<Response<List<ProcessFlowDto>>> GetAllAsync(Query query);
        Task<Response<ProcessDto>> CreateAsync(ProcessDto process, IFormFile? file, CancellationToken cancellationToken);
        Task<Response<ProcessDto>> ApproveAsync(int processId, string updatedBy, string? note, IFormFile? file, CancellationToken cancellationToken);
        Task<Response<ProcessDto>> CancelAsync(int processId, string updatedBy);

    }

}