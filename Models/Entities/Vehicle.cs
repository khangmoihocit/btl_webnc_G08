using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int CommunityId { get; set; }

        [MaxLength(150)] public string Name { get; set; } = string.Empty;
        [MaxLength(1000)] public string? Description { get; set; }
        [MaxLength(500)] public string? Condition { get; set; }
        public ShareType ShareType { get; set; } = ShareType.Free;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0; // >= 0
        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        public virtual User Owner { get; set; } = null!;
        public virtual VehicleCategory Category { get; set; } = null!;
        public virtual Community Community { get; set; } = null!;

        public virtual ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
        public virtual ICollection<VehicleAvailability> Availabilities { get; set; } = new List<VehicleAvailability>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}