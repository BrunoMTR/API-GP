using BL.Services;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Endpoints
{
    public class UnitEndpoints
    {
        public static async Task<IResult> PostUnit([FromBody] Unit unit,
            [FromServices] IUnitMapper mapper,
            [FromServices]IUnitService unitService)
        {
            var unitDto = mapper.Map(unit);
            var newUnit = await unitService.Create(unitDto);
            if(newUnit is null)
                return Results.Conflict(new
                {
                    message = "Já existe uma unidade com esses dados."
                });

            return Results.CreatedAtRoute(
                "unitId",
                new { unitId = newUnit.Id },
                newUnit);
        }

        public static async Task<IResult>GetUnit(int unitId,
            [FromServices]IUnitService unitService)
        {
            var unit = await unitService.Retrieve(unitId);
            if (unit is null)
                return Results.NotFound();           

            return Results.Ok(unit);
        }

        public static async Task<IResult> GetAllUnits([FromServices]IUnitService unitService)
        {
            var units = await unitService.GetAllAsync();
            return Results.Ok(units);
        }

        public static async Task<IResult>DeleteUnit([FromServices] IUnitService unitService, [FromRoute] int unitId)
        {
            var existing = await unitService.Retrieve(unitId);
            if (existing is null)
                return Results.NotFound();

            await unitService.Delete(unitId);

            return Results.NoContent();
        }

        public static async Task<IResult>UpdateUnit([FromBody]Unit unit, [FromRoute] int unitId,
            [FromServices] IUnitMapper mapper,
            [FromServices] IUnitService unitService)
        {
            var existing = await unitService.Retrieve(unitId);
            if (existing is null)
                return Results.NotFound();

            var unitDto = mapper.Map(unit);
            var updatedUnit = await unitService.Update(unitId, unitDto);
            if(updatedUnit is null)
                return Results.Conflict(new
                {
                    message = "Já existe uma unidade com esses dados."
                });
            return Results.Ok(updatedUnit);
        }
    }
}
