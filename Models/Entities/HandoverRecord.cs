using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class HandoverRecord
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime HandedOverAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        [MaxLength(1000)] public string? ConditionBefore { get; set; }
        [MaxLength(1000)] public string? ConditionAfter { get; set; }

        public virtual Booking Booking { get; set; } = null!;
    }
}