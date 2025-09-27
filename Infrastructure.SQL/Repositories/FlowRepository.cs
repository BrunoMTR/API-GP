using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.SQL.Repositories
{
    public class FlowRepository : IFlowRepository

    {
        private readonly DemoContext _demoContext;

        public FlowRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }
    
        public async Task<NormalizedNodeResponseDto> GetByApplicationAsync(int applicationId)
        {
            var nodes = await _demoContext.Node
                .AsNoTracking()
                .Include(x => x.Application)
                .Include(x => x.Origin)
                .Include(x => x.Destination)
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            if (!nodes.Any())
                return null;

            var application = nodes.First().Application;

            var unitDict = new Dictionary<int, UnitDto>();

            foreach (var node in nodes)
            {
                void AddUnit(UnitEntity unit)
                {
                    if (!unitDict.ContainsKey(unit.Id))
                    {
                        unitDict[unit.Id] = new UnitDto
                        {
                            Id = unit.Id,
                            Name = unit.Name,
                            Abbreviation = unit.Abbreviation,
                            Email = unit.Email
                        };
                    }
                }

                AddUnit(node.Origin);
                AddUnit(node.Destination);
            }

            return new NormalizedNodeResponseDto
            {
                Application = new ApplicationDto
                {
                    Id = application.Id,
                    Name = application.Name,
                    Abbreviation = application.Abbreviation,
                    Team = application.Team,
                    TeamEmail = application.TeamEmail,
                    ApplicationEmail = application.ApplicationEmail
                },
                Units = unitDict.Values.ToList(),
                Nodes = nodes.Select(n => new NodeDto
                {
                    Id = n.Id,
                    ApplicationId = n.Application.Id,
                    OriginId = n.Origin.Id,
                    DestinationId = n.Destination.Id,
                    Approvals = n.Approvals,
                    Direction = n.Direction
                }).ToList()
            };
        }     

    }
}
