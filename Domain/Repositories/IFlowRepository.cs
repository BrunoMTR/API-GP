using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFlowRepository
    {
        Task<List<NodeDto>> CreateAsync(GraphDto graph);

        Task<GraphDto> GetByApplicationIdAsync(int applicationId);

        Task DeleteByApplicationIdAsync(int applicationId);

    }
}
