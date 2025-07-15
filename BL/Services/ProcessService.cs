using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ProcessService : IProcessService
        
    {
        private IProcessRepository _processRepository;
        public ProcessService(IProcessRepository processRepository)
        {
            _processRepository = processRepository;
        }

        public Task<bool> AddDocuments(int id, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(ProcessDto dto)
        {
            return await _processRepository.CreateAsync(dto);
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProcessDto>> GetAll()
        {
            return await _processRepository.GetAllAsync();
        }

        public Task<bool> ReplaceDocument(int processId, int documentId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessDto> Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateHolder(int id, int holderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateState(int id, int stateId)
        {
            throw new NotImplementedException();
        }
        
    }
}
