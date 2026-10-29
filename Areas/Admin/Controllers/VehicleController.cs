using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using btl_webnc_G08.Data;
using btl_webnc_G08.Models.Entities;
using btl_webnc_G08.Models.ViewModels;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            ViewBag.Users = await _context.Users
                .AsNoTracking()
                .Select(u => new { u.Id, FullName = u.FullName ?? u.UserName ?? u.Email })
                .ToListAsync();

            return View();
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
        public async Task<IActionResult> GetById(int id)
        {
            var v = await _context.Vehicles
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (v == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phương tiện!" });
            }

            var model = new VehicleFormVM
            {
                Id = v.Id,
                OwnerId = v.OwnerId,
                CategoryId = v.CategoryId,
                CommunityId = v.CommunityId,
                Name = v.Name,
                Description = v.Description,
                Condition = v.Condition,
                ShareType = v.ShareType,
                Price = v.Price,
                Status = v.Status,
                ImageUrl = v.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleFormVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!", errors });
            }

            var ownerExists = await _context.Users.AnyAsync(u => u.Id == model.OwnerId);
            if (!ownerExists)
            {
                return Json(new { success = false, message = "Chủ xe không tồn tại!" });
            }

            var categoryExists = await _context.VehicleCategories.AnyAsync(c => c.Id == model.CategoryId);
            if (!categoryExists)
            {
                return Json(new { success = false, message = "Danh mục không hợp lệ!" });
            }

            var communityExists = await _context.Communities.AnyAsync(c => c.Id == model.CommunityId);
            if (!communityExists)
            {
                return Json(new { success = false, message = "Khu dân cư không hợp lệ!" });
            }

            var vehicle = new Vehicle
            {
                OwnerId = model.OwnerId,
                CategoryId = model.CategoryId,
                CommunityId = model.CommunityId,
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                Condition = model.Condition?.Trim(),
                ShareType = model.ShareType,
                Price = model.ShareType == ShareType.Free ? 0 : model.Price,
                Status = model.Status
            };

            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                vehicle.Images.Add(new VehicleImage
                {
                    ImageUrl = model.ImageUrl.Trim(),
                    IsPrimary = true
                });
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thêm mới phương tiện thành công!" });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] VehicleFormVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!", errors });
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Images)
                .FirstOrDefaultAsync(v => v.Id == model.Id);

            if (vehicle == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phương tiện cần sửa!" });
            }

            vehicle.OwnerId = model.OwnerId;
            vehicle.CategoryId = model.CategoryId;
            vehicle.CommunityId = model.CommunityId;
            vehicle.Name = model.Name.Trim();
            vehicle.Description = model.Description?.Trim();
            vehicle.Condition = model.Condition?.Trim();
            vehicle.ShareType = model.ShareType;
            vehicle.Price = model.ShareType == ShareType.Free ? 0 : model.Price;
            vehicle.Status = model.Status;

            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                var primaryImage = vehicle.Images.FirstOrDefault(i => i.IsPrimary) ?? vehicle.Images.FirstOrDefault();
                if (primaryImage != null)
                {
                    primaryImage.ImageUrl = model.ImageUrl.Trim();
                }
                else
                {
                    vehicle.Images.Add(new VehicleImage
                    {
                        ImageUrl = model.ImageUrl.Trim(),
                        IsPrimary = true
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật phương tiện thành công!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Images)
                .Include(v => v.Availabilities)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phương tiện!" });
            }

            var hasActiveBooking = await _context.Bookings.AnyAsync(b => b.VehicleId == id &&
                (b.Status == BookingStatus.Pending ||
                 b.Status == BookingStatus.Confirmed ||
                 b.Status == BookingStatus.Ongoing));

            if (hasActiveBooking)
            {
                return Json(new
                {
                    success = false,
                    message = "Không thể xóa phương tiện vì đang có lịch đặt xe chờ duyệt hoặc đang được mượn!"
                });
            }

            var hasBookingHistory = await _context.Bookings.AnyAsync(b => b.VehicleId == id);
            if (hasBookingHistory)
            {
                vehicle.Status = VehicleStatus.Inactive;
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Phương tiện đã có lịch sử đặt xe nên đã được chuyển sang trạng thái 'Ngừng hoạt động' để lưu trữ!"
                });
            }

            _context.VehicleImages.RemoveRange(vehicle.Images);
            _context.VehicleAvailabilities.RemoveRange(vehicle.Availabilities);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa phương tiện thành công!" });
        }
    }
}
