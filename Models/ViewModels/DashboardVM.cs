using System;
using System.Collections.Generic;

namespace btl_webnc_G08.Models.ViewModels
{
    public class DashboardVM
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int InUseVehicles { get; set; }
        public int MaintenanceVehicles { get; set; }

        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }

        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CompletedBookings { get; set; }
        public decimal TotalEstimatedRevenue { get; set; }

        public int TotalCommunities { get; set; }
        public int PendingReports { get; set; }

        public List<CategoryStatVM> CategoryStats { get; set; } = new();
        public List<RecentBookingVM> RecentBookings { get; set; } = new();
        public List<RecentReportVM> RecentReports { get; set; } = new();
    }

    public class CategoryStatVM
    {
        public string CategoryName { get; set; } = string.Empty;
        public int VehicleCount { get; set; }
        public int Percentage { get; set; }
    }

    public class RecentBookingVM
    {
        public int Id { get; set; }
        public string VehicleName { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal Fee { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = string.Empty;
    }

    public class RecentReportVM
    {
        public int Id { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = string.Empty;
    }
}
