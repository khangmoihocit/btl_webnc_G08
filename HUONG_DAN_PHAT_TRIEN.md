# Hướng dẫn phát triển ShareRide nhóm G08

Bản này tiếp tục trên source nhóm. Đã bổ sung migration đầu tiên, đăng ký/đăng nhập bằng Identity, duyệt tài khoản, đánh giá giao dịch hoàn thành, phản hồi một lần, báo cáo vi phạm, hiển thị thông báo và ghi kết quả báo cáo. Source cũ vẫn có các trang quản lý xe của nhóm.

## Chạy trong VS Code

Cần .NET SDK 10 và SQL Server LocalDB. Mở thư mục dự án, mở Terminal:

```powershell
SqlLocalDB start MSSQLLocalDB
dotnet restore
dotnet run --launch-profile http
```

Truy cập http://localhost:5253. Dừng ứng dụng bằng Ctrl+C. Development sử dụng database riêng `NeighborRide_G08_Dev`; ứng dụng áp dụng migration và tạo dữ liệu mẫu nếu database còn trống. Bản này dùng migration ban đầu mới: không trỏ thẳng vào database nhóm đã có dữ liệu khi chưa thống nhất cách chuyển đổi schema và sao lưu.

## Tạo admin lần đầu trong môi trường Development

Trong cùng terminal, đặt hai biến tạm rồi chạy ứng dụng. Dùng email và mật khẩu tự đặt; mật khẩu Identity cần ít nhất 8 ký tự, chữ hoa, chữ thường, số và ký tự đặc biệt.

```powershell
$env:BootstrapAdmin__Email = 'admin@example.com'
$env:BootstrapAdmin__Password = 'thay-bang-mat-khau-manh-cua-ban'
dotnet run --launch-profile http
```

Ứng dụng chỉ tạo Admin nếu email chưa tồn tại; không tự nâng quyền tài khoản đã có. Sau khi tạo, có thể xóa biến của phiên terminal với `Remove-Item Env:BootstrapAdmin__Password` và `Remove-Item Env:BootstrapAdmin__Email`. Không đưa mật khẩu thật vào mã nguồn, báo cáo hoặc file nộp.

Đăng ký tài khoản thành viên, đăng nhập Admin và vào `/Moderation` để duyệt; sau đó thành viên đăng nhập và vào `/Personal`. Chỉ hai bên của giao dịch có trạng thái Completed mới đánh giá được. Dữ liệu xe có sẵn là dữ liệu mẫu; bản này chưa có luồng đặt xe hoàn chỉnh để tạo giao dịch thật.

## Kiểm tra

`dotnet build` đã thành công. `scripts/test_personal_integration.py` dùng database Development riêng, tạo và dọn bản ghi thử, kiểm tra đăng ký, chặn tài khoản chờ duyệt, đăng nhập, đánh giá, phản hồi, gửi báo cáo, phân quyền Admin, CSRF và ghi kết quả báo cáo. Để chạy script phải khởi động app ở cổng 5269 bằng biến `ASPNETCORE_URLS` và `--no-launch-profile` như trong script.

## Phần chưa hoàn thiện

Chưa có luồng đặt xe và hoàn trả thực tế, gửi email/khôi phục mật khẩu, upload minh chứng, phân trang, kiểm duyệt ẩn/khóa đối tượng theo báo cáo, giao diện lịch sử giao dịch đầy đủ hoặc kiểm tra responsive toàn bộ. Model báo cáo nhóm hiện hỗ trợ User, Vehicle, Booking; chưa có ReportTargetType Review. Các thao tác quản lý xe của nhóm chưa có xác thực/phân quyền và CSRF phía server; không triển khai ra Internet trước khi hoàn thiện các điểm này. Dữ liệu mẫu người dùng của nhóm không có mật khẩu đăng nhập.
