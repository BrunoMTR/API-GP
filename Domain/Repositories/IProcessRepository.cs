using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProcessRepository
    {
        Task<ProcessDto> RetrieveAsync(int id);
        Task<int> CreateAsync(ProcessDto dto);
        Task<bool> UpdateStateAsync(int id, int stateId);
        Task<bool> UpdateHolderAsync(int id, int holderId);
        Task<bool> AddDocumentsAsync(int id, List<IFormFile> files);
        Task<bool> ReplaceDocumentAsync(int processId, int documentId, IFormFile file);
        Task<int> DeleteAsync(int id);
        Task<List<ProcessDto>> GetAllAsync();
    }
}
