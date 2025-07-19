using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<ApplicationDto> Create(ApplicationDto application)
        {
            return await _applicationRepository.CreateAsync(application);
        }

        public async Task Delete(int id)
        {
             await _applicationRepository.DeleteAsync(id);
        }

        public async Task<List<ApplicationDto>> GetAll()
        {
            return await _applicationRepository.GetAllAsync();
        }

        public async Task<ApplicationDto> Retrieve(int id)
        {
            return await _applicationRepository.RetrieveAsync(id);
        }

        public async Task<ApplicationDto> Update(ApplicationDto application,int id)
        {
            return await _applicationRepository.UpdateAsync(application, id);
        }
    }
}
