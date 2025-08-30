using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class FlowService : IFlowService
    {
        private readonly IFlowRepository _flowRepository;
        public FlowService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }


        public Task Delete(int applicationId)
        {
            throw new NotImplementedException();
        }

        public async Task<NormalizedNodeResponseDto?> Retrieve(int applicationId)
        {
            return await _flowRepository.GetByApplicationIdAsync(applicationId);
        }

        public async Task<ReactFlowDto?> RetrieveFlow(int applicationId)
        {
            return await _flowRepository.GetFlowByApplicationId(applicationId);
        }
    }
}
