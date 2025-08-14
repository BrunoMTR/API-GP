using Domain.DTOs;
using Domain.DTOs.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFlowService
    {
        Task<NormalizedNodeResponseDto?> Retrieve(int applicationId);
        Task<NormalizedNodeResponseDto?> Create(GraphDto graph);
        Task Delete(int applicationId);

        Task<ReactFlowDto?> RetrieveFlow(int applicationId);

    }
}
