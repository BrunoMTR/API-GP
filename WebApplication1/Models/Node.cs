



using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class Node{

        /// <summary>
        /// Foreign key for origin unit.
        /// </summary>
        public int OriginId { get; set; }


        /// <summary>
        /// Foreign key for destination unit.
        /// </summary>
        public int DestinationId { get; set; }

        /// <summary>
        /// Number of approvals required to traverse this edge (null if not applicable).
        /// </summary>
        public int Approvals { get; set; }

        /// <summary>
        /// Direction of the transition (e.g., Forward or Backward).
        /// </summary>
        public string Direction { get; set; }

    }
}
