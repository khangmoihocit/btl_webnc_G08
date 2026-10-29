using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using btl_webnc_G08.Data;
using btl_webnc_G08.Models.ViewModels;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalVehicles = await _context.Vehicles.CountAsync();
            var availableVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Available);
            var inUseVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.InUse);
            var maintenanceVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Maintenance);

            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.MemberStatus == MemberStatus.Active);

            var totalBookings = await _context.Bookings.CountAsync();
            var pendingBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
            var completedBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Completed);
            var revenue = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Completed || b.Status == BookingStatus.Ongoing)
                .SumAsync(b => b.EstimatedFee);

            var totalCommunities = await _context.Communities.CountAsync();
            var pendingReports = await _context.ViolationReports
                .CountAsync(r => r.Status == ReportStatus.Pending || r.Status == ReportStatus.Investigating);

            var categoryCounts = await _context.VehicleCategories
                .Select(c => new { c.Name, Count = c.Vehicles.Count })
                .ToListAsync();

            int divisor = totalVehicles > 0 ? totalVehicles : 1;
            var categoryStats = categoryCounts.Select(c => new CategoryStatVM
            {
                CategoryName = c.Name,
                VehicleCount = c.Count,
                Percentage = (int)Math.Round((double)c.Count * 100 / divisor)
            }).ToList();

            var rawRecentBookings = await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Borrower)
                .OrderByDescending(b => b.CreatedAt)
                .Take(6)
                .ToListAsync();

            var recentBookings = rawRecentBookings
                .Select(b => new RecentBookingVM
                {
                    Id = b.Id,
                    VehicleName = b.Vehicle.Name,
                    BorrowerName = b.Borrower.FullName ?? b.Borrower.UserName ?? "Chưa rõ",
                    StartAt = b.StartAt,
                    EndAt = b.EndAt,
                    Fee = b.EstimatedFee,
                    StatusName = b.Status switch
                    {
                        BookingStatus.Pending => "Chờ duyệt",
                        BookingStatus.Confirmed => "Đã duyệt",
                        BookingStatus.Ongoing => "Đang sử dụng",
                        BookingStatus.Completed => "Hoàn thành",
                        BookingStatus.Cancelled => "Đã hủy",
                        BookingStatus.Rejected => "Từ chối",
                        _ => b.Status.ToString()
                    },
                    StatusBadgeClass = b.Status switch
                    {
                        BookingStatus.Completed => "bg-success",
                        BookingStatus.Ongoing => "bg-primary",
                        BookingStatus.Confirmed => "bg-info text-dark",
                        BookingStatus.Pending => "bg-warning text-dark",
                        _ => "bg-secondary"
                    }
                })
                .ToList();

            var rawRecentReports = await _context.ViolationReports
                .Include(r => r.Reporter)
                .OrderByDescending(r => r.Id)
                .Take(5)
                .ToListAsync();

            var recentReports = rawRecentReports
                .Select(r => new RecentReportVM
                {
                    Id = r.Id,
                    ReporterName = r.Reporter.FullName ?? r.Reporter.UserName ?? "Chưa rõ",
                    TargetType = r.TargetType switch
                    {
                        ReportTargetType.Vehicle => "Phương tiện",
                        ReportTargetType.User => "Người dùng",
                        ReportTargetType.Booking => "Đơn đặt xe",
                        _ => r.TargetType.ToString()
                    },
                    Reason = r.Reason,
                    StatusName = r.Status switch
                    {
                        ReportStatus.Pending => "Chờ xử lý",
                        ReportStatus.Investigating => "Đang kiểm tra",
                        ReportStatus.Resolved => "Đã giải quyết",
                        ReportStatus.Dismissed => "Đã bác bỏ",
                        _ => r.Status.ToString()
                    },
                    StatusBadgeClass = r.Status switch
                    {
                        ReportStatus.Resolved => "bg-success",
                        ReportStatus.Investigating => "bg-info text-dark",
                        ReportStatus.Pending => "bg-warning text-dark",
                        _ => "bg-secondary"
                    }
                })
                .ToList();

            var model = new DashboardVM
            {
                TotalVehicles = totalVehicles,
                AvailableVehicles = availableVehicles,
                InUseVehicles = inUseVehicles,
                MaintenanceVehicles = maintenanceVehicles,
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalBookings = totalBookings,
                PendingBookings = pendingBookings,
                CompletedBookings = completedBookings,
                TotalEstimatedRevenue = revenue,
                TotalCommunities = totalCommunities,
                PendingReports = pendingReports,
                CategoryStats = categoryStats,
                RecentBookings = recentBookings,
                RecentReports = recentReports
            };

            return View(model);
        }
    }
}
