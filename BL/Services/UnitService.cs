using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;

        public UnitService(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }
        public async Task<UnitDto> Create(UnitDto unit)
        {
            var allUnits = await _unitRepository.GetAllUnitsAsync();

            var duplicate = allUnits.FirstOrDefault(u =>
                string.Equals(u.Name, unit.Name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit.Abbreviation, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Email, unit.Email, StringComparison.OrdinalIgnoreCase));

            if (duplicate is not null)
            {
                return null;
            }
            return await _unitRepository.CreateAsync(unit);
        }


        public async Task Delete(int unitId)
        {
            await _unitRepository.DeleteAsync(unitId);
        }

        public async Task<List<UnitDto>> GetAll()
        {
            return await _unitRepository.GetAllUnitsAsync();
        }

        public Task<UnitDto> Retrieve(int unitId)
        {
            return _unitRepository.RetrieveAsync(unitId);
        }

        public async Task<UnitDto> Update(int unitId, UnitDto unit)
        {
            var existingUnit = await _unitRepository.RetrieveAsync(unitId);
            if (existingUnit is null)
                return null;

            var allUnits = await _unitRepository.GetAllUnitsAsync();

            var duplicate = allUnits.FirstOrDefault(u =>
                u.Id != unitId &&
                (string.Equals(u.Name, unit.Name, StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(u.Abbreviation, unit.Abbreviation, StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(u.Email, unit.Email, StringComparison.OrdinalIgnoreCase)));

            if (duplicate is not null)
            {
                return null;
            }

            return await _unitRepository.UpdateAsync(unitId, unit);
        }

    }
}
