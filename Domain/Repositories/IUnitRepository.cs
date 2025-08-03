using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitRepository
    {
        Task<UnitDto> CreateAsync(UnitDto unit);
        Task<UnitDto> RetrieveAsync(int unitId);
        Task<UnitDto> UpdateAsync(int unitId, UnitDto unit);
        Task<List<UnitDto>> GetAllUnitsAsync();
        Task DeleteAsync(int unitId);
    }
}
