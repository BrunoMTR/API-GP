using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IApplicationService
    {
        Task<ApplicationDto> Retrieve(int id);
        Task<List<ApplicationDto>> GetAll();
        Task Delete(int id);
        Task<ApplicationDto> Update(ApplicationDto application,int id);
        Task<NormalizedNodeResponseDto> Create(ApplicationDto application, GraphDto graph);
    }
}
