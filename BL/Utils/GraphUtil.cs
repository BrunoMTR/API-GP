using Domain.DTOs;
using Domain.DTOs.Flow;
using Domain.DTOs.FlowDTOs;
using Infrastructure.SQL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utils
{
    public static class GraphUtil
    {
        /// <summary>
        /// Retorna o nó inicial de um fluxo, ou null se não houver.
        /// Nó inicial é definido como aquele cujo OriginId aparece apenas uma vez.
        /// </summary>
        public static NodeDto? GetInitialNode(IEnumerable<NodeDto> nodes)
        {
            if (nodes == null || !nodes.Any())
                return null;

            return nodes
                .GroupBy(n => n.OriginId)
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .FirstOrDefault();
        }

        /// <summary>
        /// Retorna o nó atual baseado no processo e na direção "AVANÇO".
        /// </summary>
        public static NodeDto? GetCurrentNode(IEnumerable<NodeDto> nodes, int currentAt)
        {
            if (nodes == null || !nodes.Any())
                return null;

            return nodes.FirstOrDefault(n =>
                n.OriginId == currentAt &&
                n.Direction.Equals("AVANÇO", System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Calcula o total de approvals necessários no fluxo (somente direção AVANÇO).
        /// </summary>
        public static int GetTotalRequiredApprovals(IEnumerable<NodeDto> nodes)
        {
            if (nodes == null || !nodes.Any())
                return 0;

            return nodes
                .Where(n => n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase))
                .Sum(n => n.Approvals);
        }

        /// <summary>
        /// Determina se o processo completará quorum no nó atual.
        /// </summary>
        public static bool HasReachedApprovalQuorum(NodeDto currentNode, int currentApprovals)
        {
            if (currentNode == null) return false;
            return (currentApprovals + 1) >= currentNode.Approvals;
        }

        /// <summary>
        /// Busca o edge de avanço a partir de um nó inicial.
        /// </summary>
        public static NodeDto? GetInitialEdge(IEnumerable<NodeDto> nodes, int initialNode)
        {
            if (nodes == null || !nodes.Any())
                return null;

            return nodes.FirstOrDefault(n =>
                n.OriginId == initialNode &&
                n.Direction.Equals("AVANÇO", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Busca o proximo node de aprovação.
        /// </summary>
        public static int GetNextNodeId(NodeDto currentNode)
        {
            if (currentNode == null)
                throw new ArgumentNullException(nameof(currentNode));
            return currentNode.DestinationId;
        }


        /// <summary>
        /// Map to flow
        /// </summary>
        public static ReactFlowDto MapToFlow(NormalizedNodeResponseDto graph)
        {
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

        /// <summary>
        /// Map to process history
        /// </summary>
        public static ProcessFlowDto MapToHistory(
            ProcessDto process,
            ReactFlowDto flow,
            int totalCount)
        {
            if (flow == null) throw new ArgumentNullException(nameof(flow));

            var visitedAtValues = process.Histories?
                .OrderBy(h => h.UpdatedAt)
                .Select(h => h.At)
                .ToHashSet() ?? new HashSet<int>();

            var currentAt = process.At;

            var nodes = flow.Nodes.Select(n =>
            {
                int nodeId = int.Parse(n.Id.Substring(1));
                var status = visitedAtValues.Contains(nodeId) ? "visited" : "pending";
                if (nodeId == currentAt) status = "current";

                return new ReactFlowNodeDto
                {
                    Id = n.Id,
                    Position = n.Position,
                    Type = n.Type,
                    Data = new NodeDataDto
                    {
                        Label = n.Data.Label,
                        Status = status
                    }
                };
            }).ToList();

            return new ProcessFlowDto
            {
                Id = process.Id,
                CreatedAt = process.CreatedAt,
                CreatedBy = process.CreatedBy,
                Unit = process.Unit == null ? null : new UnitDto
                {
                    Id = process.Unit.Id,
                    Name = process.Unit.Name,
                    Abbreviation = process.Unit.Abbreviation,
                    Email = process.Unit.Email
                },
                Application = process.Application == null ? null : new ApplicationDto
                {
                    Id = process.Application.Id,
                    Name = process.Application.Name,
                    Abbreviation = process.Application.Abbreviation,
                    Team = process.Application.Team,
                    TeamEmail = process.Application.TeamEmail,
                    ApplicationEmail = process.Application.ApplicationEmail
                },
                Status = process.Status,
                Nodes = nodes,
                Edges = flow.Edges,
                ProcessCount = totalCount
            };
        }
    }
}


