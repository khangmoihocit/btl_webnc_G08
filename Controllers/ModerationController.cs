using btl_webnc_G08.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using btl_webnc_G08.Models.Entities;
using static btl_webnc_G08.Models.Enums.AppEnums;
namespace btl_webnc_G08.Controllers;
[Authorize(Roles="Admin")]
public class ModerationController(ApplicationDbContext db,UserManager<User> users) : Controller {
 public async Task<IActionResult> Index(){ViewBag.Pending=await db.Users.Where(u=>u.MemberStatus==MemberStatus.Pending).ToListAsync();ViewBag.Reports=await db.ViolationReports.Include(r=>r.Reporter).OrderByDescending(r=>r.CreatedAt).Take(200).ToListAsync();return View();}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Verify(string id,bool approve){var u=await users.FindByIdAsync(id);if(u==null)return NotFound();if(await users.IsInRoleAsync(u,"Admin"))return Forbid();if(u.MemberStatus!=MemberStatus.Pending)return Conflict("Tài khoản không còn ở trạng thái chờ duyệt.");u.MemberStatus=approve?MemberStatus.Active:MemberStatus.Inactive;var result=await users.UpdateSecurityStampAsync(u);if(!result.Succeeded)return Conflict("Không thể cập nhật tài khoản.");db.Notifications.Add(new(){UserId=u.Id,Content=approve?"Tài khoản đã được duyệt.":"Tài khoản không được duyệt."});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Resolve(int id,ReportStatus status,string note,string version){if(status is not (ReportStatus.Investigating or ReportStatus.Resolved or ReportStatus.Dismissed)||string.IsNullOrWhiteSpace(note)||note.Trim().Length<10||note.Length>1000)return BadRequest("Trạng thái hoặc nội dung không hợp lệ.");var r=await db.ViolationReports.SingleOrDefaultAsync(x=>x.Id==id);if(r==null)return NotFound();if(r.Status is ReportStatus.Resolved or ReportStatus.Dismissed)return Conflict("Báo cáo đã kết thúc.");byte[] token;try{token=Convert.FromBase64String(version);}catch(FormatException){return BadRequest("Phiên bản không hợp lệ.");}db.Entry(r).Property(x=>x.Version).OriginalValue=token;r.Status=status;r.AdminNote=note.Trim();r.HandledAt=DateTime.UtcNow;db.Notifications.Add(new(){UserId=r.ReporterId,Content="Báo cáo #"+id+" được cập nhật."});try{await db.SaveChangesAsync();}catch(DbUpdateConcurrencyException){return Conflict("Báo cáo đã thay đổi. Vui lòng tải lại.");}return RedirectToAction(nameof(Index));}
}
