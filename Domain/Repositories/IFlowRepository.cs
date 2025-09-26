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
        Task<NormalizedNodeResponseDto> GetByApplicationAsync(int applicationId);

    }
}
