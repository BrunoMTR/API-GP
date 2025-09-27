using Domain.DTOs;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IApplicationService
    {
        Task<Response<ApplicationDto>> RetrieveAsync(int id);
        Task<List<ApplicationDto>> GetAllAsync();
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<ApplicationDto>> UpdateAsync(ApplicationDto application,int id);
        Task<Response<ApplicationDto>> CreateAsync(ApplicationDto application,GraphDto graph);
    }
}
