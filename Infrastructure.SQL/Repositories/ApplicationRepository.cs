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

        public async Task<ApplicationDto> CreateAsync(ApplicationDto application)
        {
            var applicationEntity = new ApplicationEntity
            {
                Id = application.Id,
                Name = application.Name,
                Abbreviation = application.Abbreviation,
                Team = application.Team,
                TeamEmail = application.TeamEmail,
                ApplicationEmail = application.ApplicationEmail,

            };
            await _demoContext.AddAsync(applicationEntity);
            await _demoContext.SaveChangesAsync();
            application.Id = applicationEntity.Id;
            return application;
        }

        public async Task DeleteAsync(int id)
        {
             await _demoContext.Application
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
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

        public async Task<ApplicationDto> RetrieveAsync(int id)
        {
            return await _demoContext.Application
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
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _demoContext.Application.AnyAsync(a => a.Name == name);
        }

        public async Task<bool> ExistsByAbbreviation(string abbreviation)
        {
            return await _demoContext.Application.AnyAsync(a => a.Abbreviation == abbreviation);
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
                return application;

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

        public async Task<bool> ExistsByNameExceptId(string name, int id)
        {
            return await _demoContext.Application.AnyAsync(a => a.Name == name && a.Id != id);
        }

        public async Task<bool> ExistsByAbbreviationExceptId(string abbreviation, int id)
        {
            return await _demoContext.Application.AnyAsync(a => a.Abbreviation == abbreviation && a.Id != id);
        }


    }
}
