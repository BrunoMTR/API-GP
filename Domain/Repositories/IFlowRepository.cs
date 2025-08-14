using Domain.DTOs;
using Domain.DTOs.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFlowRepository
    {
        Task<NormalizedNodeResponseDto?> CreateAsync(GraphDto graph);

        Task<NormalizedNodeResponseDto?> GetByApplicationIdAsync(int applicationId);

        Task DeleteByApplicationIdAsync(int applicationId);

        Task<ReactFlowDto?> GetFlowByApplicationId(int applicationId);

    }
}
