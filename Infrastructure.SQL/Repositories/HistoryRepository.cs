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
    public class HistoryRepository : IHistoryRepository
    {

        private readonly DemoContext _demoContext;
        public HistoryRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public async Task<HistoryDto> CreateAsync(HistoryDto historyDto)
        {
            var historyEntity = new HistoryEntity
            {
                Id = historyDto.Id,
                ApplicationId = historyDto.ApplicationId,
                ProcessId = historyDto.ProcessId,
                At = historyDto.At,
                UpdatedBy = historyDto.UpdatedBy,
                UpdatedAt = historyDto.UpdatedAt,
                Notified = historyDto.Notified
            };

            await _demoContext.AddAsync(historyEntity);
            await _demoContext.SaveChangesAsync();
            historyDto.Id = historyEntity.Id;
            return historyDto;
        }

        public async Task<List<HistoryDto>> GetAllByApplicationIdAsync(int applicationId)
        {
            return await _demoContext.History.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .Select(x => new HistoryDto
                {

                    Id = x.Id,
                    ApplicationId = x.ApplicationId,
                    ProcessId = x.ProcessId,
                    At = x.At,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    Notified = x.Notified


                }).ToListAsync();
        }

        public async Task MarkAsNotifiedAsync(int historyId)
        {
            var historyEntity = await _demoContext.History
                .FirstOrDefaultAsync(h => h.Id == historyId);

            if (historyEntity != null)
            {
                historyEntity.Notified = true;
                await _demoContext.SaveChangesAsync();
            }
        }
    }

}
