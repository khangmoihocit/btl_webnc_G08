using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Models.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string BorrowerId { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedFee { get; set; } = 0;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual User Borrower { get; set; } = null!;
        public virtual HandoverRecord? HandoverRecord { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}