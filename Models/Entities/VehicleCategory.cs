using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class VehicleCategory
    {
        public int Id { get; set; }
        [MaxLength(100)] 
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)] 
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}