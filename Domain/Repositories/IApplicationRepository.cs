using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IApplicationRepository
    {
        Task<ApplicationDto> RetrieveAsync(int id);
        Task<List<ApplicationDto>> GetAllAsync();
        Task<ApplicationDto> UpdateAsync(ApplicationDto application, int id);
        Task DeleteAsync(int id);
        Task<NormalizedNodeResponseDto> CreateAsync(ApplicationDto application, GraphDto graph);


    }
}
