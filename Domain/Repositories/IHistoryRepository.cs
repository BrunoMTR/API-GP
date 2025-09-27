using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IHistoryRepository
    {
        Task<List<HistoryDto>> GetAllByApplicationIdAsync(int applicationId);
        Task<HistoryDto> CreateAsync(HistoryDto historyDto);
        Task MarkAsNotifiedAsync(int historyId);
    }
}
