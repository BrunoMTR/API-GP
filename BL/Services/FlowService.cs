using BL.Utils;
using Domain.DTOs.Flow;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;


namespace BL.Services
{
    public class FlowService : IFlowService
    {
        private readonly IFlowRepository _flowRepository;
        public FlowService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        public async Task<Response<ReactFlowDto>> RetrieveAsync(int applicationId)
        {
            var nodes = await _flowRepository.GetByApplicationAsync(applicationId);
            if (nodes is null)
                return Response<ReactFlowDto>.Fail("No nodes found for the given application ID.");

            var flow = GraphUtil.MapToFlow(nodes);
            if (flow is null)
                return Response<ReactFlowDto>.Fail("Failed to map nodes to flow.");

            return Response<ReactFlowDto>.Ok(flow);
        }
    }
}
