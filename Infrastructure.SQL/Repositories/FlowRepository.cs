using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Repositories
{
    public class FlowRepository : IFlowRepository

    {
        private readonly DemoContext _demoContext;

        public FlowRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public async Task DeleteByApplicationIdAsync(int applicationId)
        {
            await _demoContext.Node
                .Where(x => x.ApplicationId == applicationId)
                .ExecuteDeleteAsync();
        }
        public async Task<NormalizedNodeResponseDto?> GetByApplicationIdAsync(int applicationId)
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

        public async Task<ReactFlowDto?> GetFlowByApplicationId(int applicationId)
        {
            var graph = await GetByApplicationIdAsync(applicationId);
            if (graph is null) return null;

            var reactFlowDto = new ReactFlowDto();


            int yStep = 50;
            int xStep = 300;
            var unitPositions = new Dictionary<int, PositionDto>();
            int index = 0;

            foreach (var unit in graph.Units)
            {
                unitPositions[unit.Id] = new PositionDto
                {
                    X = index * xStep,
                    Y = 0
                };
                index++;
            }


            reactFlowDto.Nodes = graph.Units.Select(u => new ReactFlowNodeDto
            {
                Id = $"n{u.Id}",
                Position = unitPositions[u.Id],
                Type = "department",
                Data = new NodeDataDto
                {
                    Label = u.Name
                }
            }).ToList();

            reactFlowDto.Edges = graph.Nodes
                .Where(n => n.Direction == "AVANÇO")
                .Select(n => new ReactFlowEdgeDto
                {
                    Id = $"e{n.Id}",
                    Source = $"n{n.OriginId}",
                    Target = $"n{n.DestinationId}",
                    Label = $"Aprovações: {n.Approvals}"
                })
            .ToList();

            return reactFlowDto;
        }

    }
}
