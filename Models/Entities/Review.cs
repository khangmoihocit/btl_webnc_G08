using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string ReviewerId { get; set; } = string.Empty;
        public string RevieweeId { get; set; } = string.Empty;

        public int Rating { get; set; } // 1 - 5
        [MaxLength(1000)] public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Booking Booking { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;
        public virtual User Reviewee { get; set; } = null!;
    }
}