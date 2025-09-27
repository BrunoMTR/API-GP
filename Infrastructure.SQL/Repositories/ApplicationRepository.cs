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

namespace Infrastructure.SQL.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly DemoContext _demoContext;
        public ApplicationRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedCount = await _demoContext.Application
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            return deletedCount > 0; 
        }


        public async Task<List<ApplicationDto>> GetAllAsync()
        {
            return await _demoContext.Application
                 .AsNoTracking()
                 .Select(x => new ApplicationDto
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Abbreviation = x.Abbreviation,
                     Team = x.Team,
                     TeamEmail = x.TeamEmail,
                     ApplicationEmail = x.ApplicationEmail

                 }).ToListAsync();
        }

        public async Task<ApplicationDto?> RetrieveAsync(int id)
        {
            var application = await _demoContext.Application
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ApplicationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation,
                    Team = x.Team,
                    TeamEmail = x.TeamEmail,
                    ApplicationEmail = x.ApplicationEmail
                })
                .FirstOrDefaultAsync();

            return application;
        }


        public async Task<ApplicationDto> UpdateAsync(ApplicationDto application, int id)
        {
            var entity = await _demoContext.Application.FindAsync(id);

            bool hasChanges =
                entity.Name != application.Name ||
                entity.Abbreviation != application.Abbreviation ||
                entity.Team != application.Team ||
                entity.TeamEmail != application.TeamEmail ||
                entity.ApplicationEmail != application.ApplicationEmail;

            if (!hasChanges)
                return null;

            entity.Name = application.Name;
            entity.Abbreviation = application.Abbreviation;
            entity.Team = application.Team;
            entity.TeamEmail = application.TeamEmail;
            entity.ApplicationEmail = application.ApplicationEmail;

            _demoContext.Application.Update(entity);
            await _demoContext.SaveChangesAsync();
            application.Id = entity.Id;

            return application;
        }

        public async Task<ApplicationDto> CreateAsync(ApplicationDto application, GraphDto graph)
        {
            var strategy = _demoContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _demoContext.Database.BeginTransactionAsync();

                try
                {
                    // Criar a entidade Application
                    var applicationEntity = new ApplicationEntity
                    {
                        Name = application.Name,
                        Abbreviation = application.Abbreviation,
                        Team = application.Team,
                        TeamEmail = application.TeamEmail,
                        ApplicationEmail = application.ApplicationEmail
                    };

                    await _demoContext.Application.AddAsync(applicationEntity);
                    await _demoContext.SaveChangesAsync();

                    // Atualizar o Id no DTO
                    application.Id = applicationEntity.Id;

                    // Associar o ApplicationId no graph
                    graph.ApplicationId = applicationEntity.Id;

                    // Criar os nodes
                    var nodesToAdd = graph.Nodes.Select(node => new NodeEntity
                    {
                        ApplicationId = graph.ApplicationId,
                        OriginId = node.OriginId,
                        DestinationId = node.DestinationId,
                        Approvals = node.Approvals,
                        Direction = node.Direction
                    }).ToList();

                    _demoContext.Node.AddRange(nodesToAdd);
                    await _demoContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new ApplicationDto
                    {
                        Id = applicationEntity.Id,
                        Name = applicationEntity.Name,
                        Abbreviation = applicationEntity.Abbreviation,
                        Team = applicationEntity.Team,
                        TeamEmail = applicationEntity.TeamEmail,
                        ApplicationEmail = applicationEntity.ApplicationEmail
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return null;
                }
            });
        }

    }
}
