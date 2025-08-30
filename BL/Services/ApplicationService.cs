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

        public async Task<NormalizedNodeResponseDto?> Create(ApplicationDto application, GraphDto graph)
        {
            var extendedNodes = new List<NodeDto>();
            foreach (var node in graph.Nodes)
            {
                extendedNodes.Add(new NodeDto
                {
                    OriginId = node.OriginId,
                    DestinationId = node.DestinationId,
                    Direction = "AVANÇO",
                    Approvals = node.Approvals
                });

                extendedNodes.Add(new NodeDto
                {
                    OriginId = node.DestinationId,
                    DestinationId = node.OriginId,
                    Direction = "RECUO",
                    Approvals = 0
                });
            }

            var newGraph = new GraphDto
            {
                ApplicationId = application.Id,
                Nodes = extendedNodes
            };

            var newApplication = await _applicationRepository.CreateAsync(application, newGraph);
     
            return newApplication;
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

        public async Task<ApplicationDto?> Update(ApplicationDto application, int applicationId)
        {
            var existingApplication = await _applicationRepository.RetrieveAsync(applicationId);
            if (existingApplication is null)
                return null;

            var allApplications = await _applicationRepository.GetAllAsync();

            var duplicated = allApplications.FirstOrDefault(a =>
                a.Id != applicationId && (
                    string.Equals(a.Name, application.Name, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.Abbreviation, application.Abbreviation, StringComparison.OrdinalIgnoreCase)
                ));

            if (duplicated is not null)
                return new ApplicationDto(); 

            return await _applicationRepository.UpdateAsync(application, applicationId);
        }

    }
}
