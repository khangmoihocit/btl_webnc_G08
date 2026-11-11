using System.Text.Json.Serialization;
using btl_webnc_G08.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using btl_webnc_G08.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

//Cấu hình DbContext kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Account/Login"; options.Events.OnRedirectToAccessDenied = context => { context.Response.StatusCode=403; return Task.CompletedTask; }; });
builder.Services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);
builder.Services.AddRazorPages();

//Thêm ControllersWithViews và cấu hình serialize Enum sang String cho JSON / AJAX
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

//Khởi tạo dữ liệu mẫu nếu có
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    if (app.Environment.IsDevelopment())
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        DbInitializer.Seed(context);
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roles.RoleExistsAsync("Admin")) await roles.CreateAsync(new IdentityRole("Admin"));
        var adminEmail = builder.Configuration["BootstrapAdmin:Email"];
        var adminPassword = builder.Configuration["BootstrapAdmin:Password"];
        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword)) {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null) {
                admin = new User { UserName=adminEmail, Email=adminEmail, FullName="Quản trị viên", MemberStatus=btl_webnc_G08.Models.Enums.AppEnums.MemberStatus.Active };
                var create = await userManager.CreateAsync(admin, adminPassword);
                if (!create.Succeeded) throw new InvalidOperationException("Không tạo được quản trị viên: " + string.Join(", ", create.Errors.Select(e => e.Description)));
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "DbInitializer chưa thể seed dữ liệu (có thể do chưa chạy migration).");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
