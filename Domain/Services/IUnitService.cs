using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IUnitService
    {
        Task<UnitDto> Retrieve(int unitId);
        Task<UnitDto> Create(UnitDto unit);
        Task Delete(int unitId);
        Task<List<UnitDto>> GetAllAsync();
        Task<UnitDto> Update(int unitId, UnitDto unit);
        Task<List<int>> GetExistingUnitIdsAsync(List<int> ids);
    }
}
