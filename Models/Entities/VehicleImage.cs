using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class VehicleImage
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        [MaxLength(500)] public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;

        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}