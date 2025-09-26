using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFlowService
    {
        Task<Response<ReactFlowDto>> RetrieveAsync(int applicationId);
    }
}
