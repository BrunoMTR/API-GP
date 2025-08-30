using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class NodeEntity
    {
        public int Id { get; set; }

        public ApplicationEntity Application { get; set; }

        public int ApplicationId { get; set; }

        public UnitEntity Origin { get; set; }

        public int OriginId { get; set; }

        public UnitEntity Destination { get; set; }

        public int DestinationId { get; set; }

        public int Approvals { get; set; }

        public string Direction { get; set; }
    }
}
