# Nhật ký thay đổi cá nhân — ShareRide G08

Ngày: 11/11/2026. Thư mục: `C:\Users\acer\Downloads\btl_webnc_G08 (1)`. Nhánh hiện tại khi kiểm tra: `main`.

## Các chức năng đã bổ sung hoặc chỉnh sửa

- Đăng ký, đăng nhập, đăng xuất bằng Identity; tài khoản mới chờ Admin duyệt. Hồ sơ cá nhân cho phép cập nhật họ tên và số điện thoại; số điện thoại bắt buộc 10 chữ số, bắt đầu bằng 0 và không trùng.
- Đánh giá giao dịch hoàn thành, phản hồi của người nhận, báo cáo vi phạm, xem báo cáo và thông báo. Admin duyệt tài khoản và cập nhật trạng thái/kết quả báo cáo.
- Thêm migration ban đầu và migration chỉ số duy nhất cho số điện thoại. Development sử dụng LocalDB riêng `NeighborRide_G08_Dev`. Bỏ thao tác xóa xe có sẵn khi tạo dữ liệu mẫu.
- Gom các nút tài khoản vào menu ba gạch; đổi tên nút thành “Đánh giá”. Sửa thông báo đặt thuê để không báo thành công khi chức năng chưa gửi yêu cầu.
- Thêm hướng dẫn chạy và script kiểm thử tích hợp. `dotnet build` đã đạt; script đã kiểm tra tài khoản, hồ sơ, số điện thoại, đánh giá, phản hồi, báo cáo, quyền Admin, CSRF và dọn dữ liệu thử.

## Tệp đã chỉnh sửa

- `.gitignore` — cho phép lưu migration.
- `Data/ApplicationDbContext.cs` — ràng buộc số điện thoại duy nhất.
- `Data/DbInitializer.cs` — tránh xóa dữ liệu xe khi seed.
- `Models/Entities/Review.cs` — nội dung và thời gian phản hồi, trạng thái ẩn.
- `Models/Entities/ViolationReport.cs` — thời gian và phiên bản xử lý báo cáo.
- `Program.cs` — Identity, phân quyền, cookie và khởi tạo Admin Development theo biến môi trường.
- `Views/Shared/_Layout.cshtml` — liên kết đăng nhập và menu tài khoản.
- `Views/Vehicle/Index.cshtml` — thông báo đặt thuê đúng trạng thái.
- `appsettings.Development.json` — LocalDB phát triển riêng.
- `btl_webnc_G08.csproj` — gói EF Design và cập nhật Pkcs.
- `wwwroot/css/site.css` — kiểu menu tài khoản và responsive.

## Tệp mới

- `Controllers/AccountController.cs` — đăng ký, đăng nhập, đăng xuất, hồ sơ.
- `Controllers/ModerationController.cs` — duyệt tài khoản, xử lý báo cáo.
- `Controllers/PersonalController.cs` — đánh giá, phản hồi, báo cáo và thông báo.
- `Data/DesignTimeDbContextFactory.cs` — cấu hình EF khi tạo migration.
- `Models/ViewModels/PersonalVM.cs` — dữ liệu và validation biểu mẫu.
- `Migrations/20261111115737_InitialTeamIntegration.cs` và `.Designer.cs` — migration ban đầu.
- `Migrations/20261111120847_UniqueUserPhone.cs` và `.Designer.cs` — ràng buộc số điện thoại.
- `Migrations/ApplicationDbContextModelSnapshot.cs` — snapshot schema.
- `Views/Account/Login.cshtml`, `Profile.cshtml`, `Register.cshtml` — giao diện tài khoản.
- `Views/Moderation/Index.cshtml` — giao diện Admin.
- `Views/Personal/Index.cshtml` — đánh giá, phản hồi và báo cáo.
- `scripts/test_personal_integration.py` — kiểm thử HTTP/SQL trên database Development riêng.
- `HUONG_DAN_PHAT_TRIEN.md` — chạy dự án, tạo Admin và giới hạn hiện tại.
- `NHAT_KY_THAY_DOI_G08.md` — nhật ký này.

## Lệnh Git trong PowerShell hoặc Terminal VS Code

```powershell
cd 'C:\Users\acer\Downloads\btl_webnc_G08 (1)'
git status
git add .gitignore Data/ApplicationDbContext.cs Data/DbInitializer.cs Models/Entities/Review.cs Models/Entities/ViolationReport.cs Program.cs Views/Shared/_Layout.cshtml Views/Vehicle/Index.cshtml appsettings.Development.json btl_webnc_G08.csproj wwwroot/css/site.css Controllers/AccountController.cs Controllers/ModerationController.cs Controllers/PersonalController.cs Data/DesignTimeDbContextFactory.cs Models/ViewModels/PersonalVM.cs Migrations Views/Account Views/Moderation Views/Personal scripts/test_personal_integration.py HUONG_DAN_PHAT_TRIEN.md NHAT_KY_THAY_DOI_G08.md
git diff --cached --stat
git diff --cached
git commit -m "Add account reviews and violation reporting"
```

Sau khi kiểm tra và commit, nếu được nhóm cho phép đẩy nhánh hiện tại lên remote:

```powershell
git push origin main
```

## Giới hạn cần trao đổi với nhóm

Chưa có luồng đặt/trả xe thực tế, khôi phục mật khẩu, upload ảnh minh chứng hoặc biện pháp ẩn/khóa đối tượng qua báo cáo. Các thao tác quản lý xe của nhóm chưa được bảo vệ đầy đủ bằng phân quyền và CSRF. Migration ban đầu này cần được nhóm rà soát trước khi áp dụng vào database đã có dữ liệu; không xóa hoặc ghi đè database hiện có.
