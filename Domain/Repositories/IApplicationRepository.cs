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
        Task<ApplicationDto> CreateAsync(ApplicationDto application);
        Task<ApplicationDto> UpdateAsync(ApplicationDto application, int id);
        Task DeleteAsync(int id);
        Task<bool> ExistsByName(string name);
        Task<bool> ExistsByAbbreviation(string abbreviation);
        Task<bool> ExistsByNameExceptId(string name, int id);
        Task<bool> ExistsByAbbreviationExceptId(string name, int id);


    }
}
