using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Mapping
{
    public class UnitMapper : IUnitMapper
    {
        public UnitDto Map(Unit unit)
        {
            return unit is not null ? new UnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation,
                Email = unit.Email
            }:null;
        }

        public Unit Map(UnitDto unitDto)
        {
            return unitDto is not null ? new Unit
            {
                Id = unitDto.Id,
                Name = unitDto.Name,
                Abbreviation = unitDto.Abbreviation,
                Email = unitDto.Email
            } : null;
        }
    }
}
