using BL.Utils;
using Domain.DTOs;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;


namespace BL.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUnitRepository _unitRepository;

        public ApplicationService(IApplicationRepository applicationRepository, IUnitRepository unitRepository)
        {
            _applicationRepository = applicationRepository;
            _unitRepository = unitRepository;
        }

        public async Task<Response<ApplicationDto>> CreateAsync(
            ApplicationDto application,
            GraphDto graph)
        {
            // Validar unidades
            var allIds = graph.Nodes
                .SelectMany(n => new[] { n.OriginId, n.DestinationId })
                .Distinct()
                .ToList();

            var existingIds = await _unitRepository.GetExistingUnitIdsAsync(allIds);
            var missingIds = allIds.Except(existingIds).ToList();

            if (missingIds.Any())
                return Response<ApplicationDto>.Fail($"UnitIds inválidos: {string.Join(", ", missingIds)}");

           
            if (!GraphUtil.HasCycle(graph.Nodes))
                return Response<ApplicationDto>.Fail("The graph must contain at least one cycle.");

            var newGraph = GraphUtil.ExtendGraph(application, graph);

            
            var newApplication = await _applicationRepository.CreateAsync(application, newGraph);
            if (newApplication is null)
                return Response<ApplicationDto>.Fail("An error occurred while creating the application.");

            return Response<ApplicationDto>.Ok(newApplication);
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            var deleted = await _applicationRepository.DeleteAsync(id);
            if (!deleted)
                return Response<bool>.Fail("Application not found");
            return Response<bool>.Ok(true, "Application deleted");
        }

        public async Task<List<ApplicationDto>> GetAllAsync()
        {
            return await _applicationRepository.GetAllAsync();
        }

        public async Task<Response<ApplicationDto>> RetrieveAsync(int id)
        {
            var application = await _applicationRepository.RetrieveAsync(id);
            if(application is null)
                return Response<ApplicationDto>.Fail("Application not found");

            return Response<ApplicationDto>.Ok(application);
        }

        public async Task<Response<ApplicationDto>> UpdateAsync(ApplicationDto application, int applicationId)
        {
            var existingApplication = await _applicationRepository.RetrieveAsync(applicationId);
            if (existingApplication is null)
                return Response<ApplicationDto>.Fail("Application not found");

            var allApplications = await _applicationRepository.GetAllAsync();

            var duplicated = allApplications.FirstOrDefault(a =>
                a.Id != applicationId && (
                    string.Equals(a.Name, application.Name, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.Abbreviation, application.Abbreviation, StringComparison.OrdinalIgnoreCase)
                ));

            if (duplicated is not null)
                return Response<ApplicationDto>.Fail("Other application with the data entered");

            var updatedApplication = await _applicationRepository.UpdateAsync(application, applicationId);
            if (updatedApplication is null)
                return Response<ApplicationDto>.Fail("Error Updating application");

            return Response<ApplicationDto>.Ok(updatedApplication, "Application updated");

        }

    }
}
