using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Models.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        public int? CommunityId { get; set; }
        public MemberStatus MemberStatus { get; set; } = MemberStatus.Pending;

        public virtual Community? Community { get; set; }
        public virtual ICollection<Vehicle> OwnedVehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<ViolationReport> SubmittedReports { get; set; } = new List<ViolationReport>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}