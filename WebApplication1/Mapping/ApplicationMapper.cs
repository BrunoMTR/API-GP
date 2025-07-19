using Domain.DTOs;
using Presentation.Mapping.Interfaces;
using Presentation.Models;

namespace Presentation.Mapping
{
    public class ApplicationMapper : IApplicationMapper
    {
        public ApplicationDto Map(Application application)
        {
            return application is not null ? new ApplicationDto
            {
                Id = application.Id,
                Name = application.Name,
                Abbreviation = application.Abbreviation,
                Team = application.Team,
                TeamEmail = application.TeamEmail,
                ApplicationEmail = application.ApplicationEmail

            }: null;
        }

        public Application Map(ApplicationDto application)
        {
            return application is not null ? new Application
            {
                Id = application.Id,
                Name = application.Name,
                Abbreviation = application.Abbreviation,
                Team = application.Team,
                TeamEmail = application.TeamEmail,
                ApplicationEmail = application.ApplicationEmail
            } : null;
        }
    }
}
