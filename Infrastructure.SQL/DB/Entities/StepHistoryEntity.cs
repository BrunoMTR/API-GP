using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB.Entities
{
    public class StepHistoryEntity
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public ProcessEntity Process { get; set; }
        public int StepId { get; set; }
        public StepEntity Step { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ExecutedById { get; set; }
        public AssociateEntity ExecutedBy { get; set; }
    }
}
