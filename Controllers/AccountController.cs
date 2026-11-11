using btl_webnc_G08.Data;
using btl_webnc_G08.Models.Entities;
using btl_webnc_G08.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static btl_webnc_G08.Models.Enums.AppEnums;
namespace btl_webnc_G08.Controllers;
public class AccountController(UserManager<User> users, SignInManager<User> signIn, ApplicationDbContext db) : Controller {
 [HttpGet] public async Task<IActionResult> Register(){ViewBag.Communities=await db.Communities.Where(c=>c.IsActive).ToListAsync();return View();}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Register(RegisterVM model){
   if(!await db.Communities.AnyAsync(c=>c.Id==model.CommunityId&&c.IsActive))ModelState.AddModelError("CommunityId","Chọn cộng đồng hợp lệ.");
   if(ModelState.IsValid && await db.Users.AnyAsync(u=>u.PhoneNumber==model.Phone))ModelState.AddModelError("Phone","Số điện thoại đã được sử dụng.");
   if(ModelState.IsValid){var user=new User{UserName=model.Email.Trim(),Email=model.Email.Trim(),FullName=model.FullName.Trim(),PhoneNumber=model.Phone,CommunityId=model.CommunityId,MemberStatus=MemberStatus.Pending};
     IdentityResult result;try{result=await users.CreateAsync(user,model.Password);}catch(DbUpdateException){ModelState.AddModelError("Phone","Số điện thoại đã được sử dụng.");ViewBag.Communities=await db.Communities.Where(c=>c.IsActive).ToListAsync();return View(model);}if(result.Succeeded){TempData["Message"]="Đăng ký thành công. Tài khoản chờ quản trị viên duyệt.";return RedirectToAction(nameof(Login));}foreach(var e in result.Errors)ModelState.AddModelError("",e.Description);}
   ViewBag.Communities=await db.Communities.Where(c=>c.IsActive).ToListAsync();return View(model);
 }
 [HttpGet] public IActionResult Login()=>View();
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginVM model){
   if(!ModelState.IsValid)return View(model);var user=await users.FindByEmailAsync(model.Email.Trim());
   if(user==null){ModelState.AddModelError("","Email hoặc mật khẩu không đúng.");return View(model);}
   var check=await signIn.CheckPasswordSignInAsync(user,model.Password,true);
   if(!check.Succeeded){ModelState.AddModelError("",check.IsLockedOut?"Tài khoản tạm khóa do nhập sai nhiều lần.":"Email hoặc mật khẩu không đúng.");return View(model);}
   if(user.MemberStatus!=MemberStatus.Active){ModelState.AddModelError("","Tài khoản chưa được duyệt hoặc đã bị khóa.");return View(model);}
   await signIn.SignInAsync(user,false);return RedirectToAction("Index","Home");
 }
 [Authorize,HttpGet] public async Task<IActionResult> Profile(){var u=await users.GetUserAsync(User);if(u==null)return Challenge();ViewBag.Email=u.Email;ViewBag.Community=await db.Communities.Where(c=>c.Id==u.CommunityId).Select(c=>c.Name).FirstOrDefaultAsync();return View(new ProfileVM{FullName=u.FullName,Phone=u.PhoneNumber??""});}
 [Authorize,HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Profile(ProfileVM model){var u=await users.GetUserAsync(User);if(u==null)return Challenge();ViewBag.Email=u.Email;ViewBag.Community=await db.Communities.Where(c=>c.Id==u.CommunityId).Select(c=>c.Name).FirstOrDefaultAsync();if(ModelState.IsValid&&await db.Users.AnyAsync(x=>x.Id!=u.Id&&x.PhoneNumber==model.Phone))ModelState.AddModelError("Phone","Số điện thoại đã được sử dụng.");if(ModelState.IsValid){u.FullName=model.FullName.Trim();u.PhoneNumber=model.Phone;try{var result=await users.UpdateAsync(u);if(result.Succeeded){TempData["Message"]="Đã cập nhật hồ sơ.";return RedirectToAction(nameof(Profile));}foreach(var e in result.Errors)ModelState.AddModelError("",e.Description);}catch(DbUpdateException){ModelState.AddModelError("Phone","Số điện thoại đã được sử dụng.");}}return View(model);}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Logout(){await signIn.SignOutAsync();return RedirectToAction("Index","Home");}
}
