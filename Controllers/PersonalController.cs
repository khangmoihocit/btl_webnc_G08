using System.Security.Claims;
using btl_webnc_G08.Data;
using btl_webnc_G08.Models.Entities;
using btl_webnc_G08.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static btl_webnc_G08.Models.Enums.AppEnums;
namespace btl_webnc_G08.Controllers;
[Authorize]
public class PersonalController(ApplicationDbContext db) : Controller {
 string UserId=>User.FindFirstValue(ClaimTypes.NameIdentifier)!;
 async Task<bool> Active()=>await db.Users.AnyAsync(u=>u.Id==UserId&&u.MemberStatus==MemberStatus.Active);
 public async Task<IActionResult> Index(){if(!await Active())return Forbid();
 ViewBag.Bookings=await db.Bookings.Include(b=>b.Vehicle).Where(b=>b.Status==BookingStatus.Completed&&b.BorrowerId!=b.Vehicle.OwnerId&&(b.BorrowerId==UserId||b.Vehicle.OwnerId==UserId)&&!db.Reviews.Any(r=>r.BookingId==b.Id&&r.ReviewerId==UserId)).ToListAsync();
 ViewBag.Reviews=await db.Reviews.Include(r=>r.Booking).ThenInclude(b=>b.Vehicle).Include(r=>r.Reviewer).Where(r=>r.ReviewerId==UserId||r.RevieweeId==UserId).OrderByDescending(r=>r.CreatedAt).ToListAsync();
 ViewBag.Notifications=await db.Notifications.Where(n=>n.UserId==UserId).OrderByDescending(n=>n.CreatedAt).Take(50).ToListAsync();ViewBag.Reports=await db.ViolationReports.Where(r=>r.ReporterId==UserId).OrderByDescending(r=>r.CreatedAt).ToListAsync();ViewBag.CurrentId=UserId;return View();}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Review(ReviewInput m){if(!await Active())return Forbid();if(!ModelState.IsValid){TempData["Error"]="Kiểm tra điểm và nội dung đánh giá.";return RedirectToAction(nameof(Index));}
 var b=await db.Bookings.Include(x=>x.Vehicle).SingleOrDefaultAsync(x=>x.Id==m.BookingId);if(b==null)return NotFound();if(b.BorrowerId!=UserId&&b.Vehicle.OwnerId!=UserId)return Forbid();
 if(b.Status!=BookingStatus.Completed||b.BorrowerId==b.Vehicle.OwnerId||await db.Reviews.AnyAsync(r=>r.BookingId==b.Id&&r.ReviewerId==UserId)){TempData["Error"]="Giao dịch chưa đủ điều kiện hoặc bạn đã đánh giá.";return RedirectToAction(nameof(Index));}
 var target=b.BorrowerId==UserId?b.Vehicle.OwnerId:b.BorrowerId;db.Reviews.Add(new(){BookingId=b.Id,ReviewerId=UserId,RevieweeId=target,Rating=m.Rating,Comment=m.Comment?.Trim()});db.Notifications.Add(new(){UserId=target,Content="Bạn nhận được đánh giá cho giao dịch "+b.Id});
 try{await db.SaveChangesAsync();TempData["Message"]="Đã lưu đánh giá.";}catch(DbUpdateException){TempData["Error"]="Không lưu được đánh giá; dữ liệu có thể đã tồn tại.";}return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Reply(int id,ReplyInput m){if(!await Active())return Forbid();var r=await db.Reviews.SingleOrDefaultAsync(x=>x.Id==id);if(r==null)return NotFound();if(r.RevieweeId!=UserId)return Forbid();
 if(!ModelState.IsValid||m.Content.Trim().Length<5||r.IsHidden||r.Reply!=null){TempData["Error"]="Phản hồi không hợp lệ hoặc đánh giá đã có phản hồi / bị ẩn.";return RedirectToAction(nameof(Index));}
 await using var tx=await db.Database.BeginTransactionAsync();
 var updated=await db.Reviews.Where(x=>x.Id==id&&x.RevieweeId==UserId&&!x.IsHidden&&x.Reply==null).ExecuteUpdateAsync(set=>set.SetProperty(x=>x.Reply,m.Content.Trim()).SetProperty(x=>x.RepliedAt,DateTime.UtcNow));
 if(updated==0){TempData["Error"]="Đánh giá đã có phản hồi hoặc không còn hiển thị.";return RedirectToAction(nameof(Index));}
 db.Notifications.Add(new(){UserId=r.ReviewerId,Content="Đánh giá của bạn đã được phản hồi."});await db.SaveChangesAsync();await tx.CommitAsync();TempData["Message"]="Đã lưu phản hồi.";return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Report(ReportInput m){if(!await Active())return Forbid();if(!ModelState.IsValid||!Enum.IsDefined(m.TargetType)||m.Reason.Trim().Length<5||m.Description.Trim().Length<10){TempData["Error"]="Điền đầy đủ thông tin báo cáo.";return RedirectToAction(nameof(Index));}
 var community=await db.Users.Where(u=>u.Id==UserId).Select(u=>u.CommunityId).SingleAsync();bool valid=m.TargetType switch{
 ReportTargetType.User=>m.TargetId!=UserId&&community!=null&&await db.Users.AnyAsync(u=>u.Id==m.TargetId&&u.CommunityId==community),
 ReportTargetType.Vehicle=>int.TryParse(m.TargetId,out var id)&&community!=null&&await db.Vehicles.AnyAsync(v=>v.Id==id&&v.CommunityId==community),
 ReportTargetType.Booking=>int.TryParse(m.TargetId,out var id)&&await db.Bookings.AnyAsync(b=>b.Id==id&&(b.BorrowerId==UserId||b.Vehicle.OwnerId==UserId)),_=>false};
 if(!valid){TempData["Error"]="Đối tượng không tồn tại hoặc bạn không có quyền báo cáo.";return RedirectToAction(nameof(Index));}
 db.ViolationReports.Add(new(){ReporterId=UserId,TargetType=m.TargetType,TargetId=m.TargetId,Reason=m.Reason.Trim(),Description=m.Description.Trim()});await db.SaveChangesAsync();TempData["Message"]="Đã gửi báo cáo.";return RedirectToAction(nameof(Index));}
}
