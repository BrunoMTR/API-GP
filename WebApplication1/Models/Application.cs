using Domain.DTOs;

namespace Presentation.Models
{
    /// <summary>
    /// Represents an application that contains processes and steps.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Unique identifier of the application.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviation of the application's name.
        /// </summary>
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Name of the team responsible for the application.
        /// </summary>
        public string? Team { get; set; }

        /// <summary>
        /// Contact email of the responsible team.
        /// </summary>
        public string? TeamEmail { get; set; }

        /// <summary>
        /// Main email address of the application.
        /// </summary>
        public string? ApplicationEmail { get; set; }

        /// <summary>
        /// Collection of processes associated with the application.
        /// </summary>
        public ICollection<Process> Processes { get; set; } = new List<Process>();

        /// <summary>
        /// Collection of steps associated with the application.
        /// </summary>
        public ICollection<Step> Steps { get; set; } = new List<Step>();
    }
}
