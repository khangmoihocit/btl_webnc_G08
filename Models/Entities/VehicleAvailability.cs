using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class VehicleAvailability
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}