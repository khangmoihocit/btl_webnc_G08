using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Enums
{
    public class AppEnums
    {
        public enum MemberStatus { Pending = 0, Active = 1, Suspended = 2, Inactive = 3 }
        public enum ShareType { Free = 0, Rental = 1 }
        public enum VehicleStatus { PendingApproval = 0, Available = 1, Reserved = 2, InUse = 3, Maintenance = 4, Inactive = 5 }
        public enum BookingStatus { Pending = 0, Confirmed = 1, Ongoing = 2, Completed = 3, Cancelled = 4, Rejected = 5 }
        public enum ReportTargetType { Vehicle = 0, User = 1, Booking = 2 }
        public enum ReportStatus { Pending = 0, Investigating = 1, Resolved = 2, Dismissed = 3 }
    }
}