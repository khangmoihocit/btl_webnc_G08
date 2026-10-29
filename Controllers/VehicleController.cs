using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using btl_webnc_G08.Data;
using btl_webnc_G08.Models.ViewModels;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.VehicleCategories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .ToListAsync();

            ViewBag.Communities = await _context.Communities
                .AsNoTracking()
                .Where(c => c.IsActive)
                .ToListAsync();

            return View();
        }

        public IActionResult Manage()
        {
            return RedirectToAction("Index", "Vehicle", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? categoryId, int? communityId, VehicleStatus? status, string? keyword, ShareType? shareType, string? sortBy)
        {
            var query = _context.Vehicles
                .AsNoTracking()
                .Include(v => v.Owner)
                .Include(v => v.Category)
                .Include(v => v.Community)
                .Include(v => v.Images)
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(v => v.CategoryId == categoryId.Value);
            }

            if (communityId.HasValue && communityId.Value > 0)
            {
                query = query.Where(v => v.CommunityId == communityId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(v => v.Status == status.Value);
            }

            if (shareType.HasValue)
            {
                query = query.Where(v => v.ShareType == shareType.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(v => v.Name.ToLower().Contains(kw) ||
                                         (v.Description != null && v.Description.ToLower().Contains(kw)) ||
                                         (v.Community != null && v.Community.Name.ToLower().Contains(kw)));
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(v => v.Price),
                "price_desc" => query.OrderByDescending(v => v.Price),
                "name" => query.OrderBy(v => v.Name),
                _ => query.OrderByDescending(v => v.Id)
            };

            var vehicles = await query
                .Select(v => new VehicleItemVM
                {
                    Id = v.Id,
                    OwnerId = v.OwnerId,
                    OwnerName = v.Owner != null ? (v.Owner.FullName ?? v.Owner.UserName ?? "Chưa rõ") : "Chưa rõ",
                    CategoryId = v.CategoryId,
                    CategoryName = v.Category != null ? v.Category.Name : "Khác",
                    CommunityId = v.CommunityId,
                    CommunityName = v.Community != null ? v.Community.Name : "Khác",
                    Name = v.Name,
                    Description = v.Description,
                    Condition = v.Condition,
                    ShareType = v.ShareType,
                    Price = v.Price,
                    Status = v.Status,
                    ImageUrl = v.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()
                })
                .ToListAsync();

            return Json(new { success = true, data = vehicles });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _context.Vehicles
                .AsNoTracking()
                .Include(v => v.Owner)
                .Include(v => v.Category)
                .Include(v => v.Community)
                .Include(v => v.Images)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phương tiện!" });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    vehicle.Id,
                    vehicle.Name,
                    vehicle.Description,
                    vehicle.Condition,
                    vehicle.Price,
                    shareType = vehicle.ShareType.ToString(),
                    shareTypeName = vehicle.ShareType == ShareType.Free ? "Miễn phí" : "Cho thuê",
                    status = vehicle.Status.ToString(),
                    statusName = vehicle.Status switch
                    {
                        VehicleStatus.PendingApproval => "Chờ duyệt",
                        VehicleStatus.Available => "Sẵn sàng",
                        VehicleStatus.Reserved => "Đã đặt trước",
                        VehicleStatus.InUse => "Đang sử dụng",
                        VehicleStatus.Maintenance => "Bảo trì",
                        VehicleStatus.Inactive => "Ngừng hoạt động",
                        _ => vehicle.Status.ToString()
                    },
                    ownerName = vehicle.Owner?.FullName ?? vehicle.Owner?.UserName ?? "Chưa rõ",
                    categoryName = vehicle.Category?.Name ?? "Khác",
                    communityName = vehicle.Community?.Name ?? "Khác",
                    images = vehicle.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).ToList()
                }
            });
        }
    }
}
