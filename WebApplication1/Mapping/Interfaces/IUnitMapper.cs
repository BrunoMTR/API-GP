using Domain.DTOs;
using Presentation.Models;

namespace Presentation.Mapping.Interfaces
{
    public interface IUnitMapper
    {
        UnitDto Map(Unit unit);
        Unit Map(UnitDto unitDto);
    }
}
