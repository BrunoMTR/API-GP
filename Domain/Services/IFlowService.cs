using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFlowService
    {
        Task<GraphDto> Retrieve(int applicationId);
        Task<List<NodeDto>> Create(GraphDto graph);
        Task Delete(int applicationId);

    }
}
