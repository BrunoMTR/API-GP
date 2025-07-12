using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IProcessService
    {
        Task<ProcessDto> Retrieve(int id);
        Task<int> Create(ProcessDto dto);
        Task<bool> UpdateState(int id, int stateId);
        Task<bool> UpdateHolder(int id, int holderId);
        Task<bool> AddDocuments(int id, List<IFormFile> files);
        Task<bool> ReplaceDocument(int processId, int documentId, IFormFile file);
        Task<bool> Delete(int id);
        Task<List<ProcessDto>> GetAll();
    }
}
