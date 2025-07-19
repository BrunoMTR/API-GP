using Domain.DTOs;
using Presentation.Models;

namespace Presentation.Mapping.Interfaces
{
    public interface IApplicationMapper
    {
        ApplicationDto Map(Application application);
        Application Map(ApplicationDto application);
    }
}
