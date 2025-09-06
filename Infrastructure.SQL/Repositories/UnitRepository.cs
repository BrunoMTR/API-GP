using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.SQL.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DemoContext _demoContext;
        public UnitRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public async Task<UnitDto> CreateAsync(UnitDto unit)
        {
            var unitEntity = new UnitEntity
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation,
                Email = unit.Email
            };

            await _demoContext.AddAsync(unitEntity);
            await _demoContext.SaveChangesAsync();
            unit.Id = unitEntity.Id;
            return unit;
        }

        public async Task DeleteAsync(int unitId)
        {
            await _demoContext.Unit
                .Where(x => x.Id == unitId)
                .ExecuteDeleteAsync();
        }

        public async Task<List<UnitDto>> GetAllUnitsAsync()
        {
            return await _demoContext.Unit.AsNoTracking()
                .Select(x => new UnitDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation,
                    Email = x.Email
                }).ToListAsync();
        }

        public async Task<List<int>> GetExistingUnitIdsAsync(List<int> ids)
        {
            return await _demoContext.Unit
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();
        }

        public async Task<UnitDto> RetrieveAsync(int unitId)
        {
            return await _demoContext.Unit.AsNoTracking()
                .Where(x => x.Id == unitId)
                .Select(x => new UnitDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation,
                    Email = x.Email
                }).FirstOrDefaultAsync();
        }

        public async Task<UnitDto> UpdateAsync(int unitId,UnitDto unit)
        {
            var entity = await _demoContext.Unit.FindAsync(unitId);
            bool hasChanges =
            entity.Name != unit.Name ||
                entity.Abbreviation != unit.Abbreviation ||
                entity.Email != unit.Email;


            if (!hasChanges)
                return unit;

            entity.Name = unit.Name;
            entity.Abbreviation = unit.Abbreviation;
            entity.Email = unit.Email;

            _demoContext.Unit.Update(entity);
            await _demoContext.SaveChangesAsync();
            unit.Id = entity.Id;

            return unit;
        }
    }
    
}
