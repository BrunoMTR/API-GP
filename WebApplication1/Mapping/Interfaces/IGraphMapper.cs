using Domain.DTOs;
using Presentation.Models;

namespace Presentation.Mapping.Interfaces
{
    public interface IGraphMapper
    {
        Graph Map(GraphDto graphDto);
        GraphDto Map(Graph graph);
    }
}
