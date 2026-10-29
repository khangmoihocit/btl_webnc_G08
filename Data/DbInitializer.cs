using System;
using System.Collections.Generic;
using System.Linq;
using btl_webnc_G08.Models.Entities;
using Microsoft.AspNetCore.Identity;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            try
            {
                if (!context.Database.CanConnect())
                {
                    return;
                }

                var existingCommunityNames = context.Communities.Select(c => c.Name).ToHashSet();
                var communitiesToAdd = new List<Community>
                {
                    new Community { Name = "Vinhomes Ocean Park", Address = "Gia Lâm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị EcoPark", Address = "Văn Giang, Hưng Yên", IsActive = true },
                    new Community { Name = "Chung cư Green Stars", Address = "234 Phạm Văn Đồng, Bắc Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư Green Bay", Address = "Mễ Trì, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư The Matrix One", Address = "Lê Quang Đạo, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị Vinhomes Smart City", Address = "Tây Mỗ, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư EcoHome 3", Address = "Đông Ngạc, Bắc Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị Ciputra Hanoi", Address = "Xuân Đỉnh, Bắc Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư Times City", Address = "458 Minh Khai, Hai Bà Trưng, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư Masteri West Heights", Address = "Tây Mỗ, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị Sala Đại Quang Minh", Address = "Thủ Thiêm, TP. Thủ Đức, TP. Hồ Chí Minh", IsActive = true },
                    new Community { Name = "Chung cư Vinhomes Central Park", Address = "208 Nguyễn Hữu Cảnh, Bình Thạnh, TP. Hồ Chí Minh", IsActive = true }
                };

                foreach (var com in communitiesToAdd)
                {
                    if (!existingCommunityNames.Contains(com.Name))
                    {
                        context.Communities.Add(com);
                    }
                }
                context.SaveChanges();

                var existingCategoryNames = context.VehicleCategories.Select(c => c.Name).ToHashSet();
                var categoriesToAdd = new List<VehicleCategory>
                {
                    new VehicleCategory { Name = "Xe đạp", Description = "Xe đạp cơ, xe đạp dạo phố và xe đạp gấp gọn", IsActive = true },
                    new VehicleCategory { Name = "Xe đạp điện / Xe máy điện", Description = "Phương tiện chạy điện thân thiện môi trường", IsActive = true },
                    new VehicleCategory { Name = "Xe máy", Description = "Xe tay ga, xe số các loại", IsActive = true },
                    new VehicleCategory { Name = "Ô tô điện", Description = "Xe ô tô điện di chuyển nội khu và lân cận", IsActive = true },
                    new VehicleCategory { Name = "Ô tô", Description = "Xe ô tô 4-7 chỗ, sedan, SUV, siêu xe thể thao", IsActive = true },
                    new VehicleCategory { Name = "Xe đạp thể thao / Địa hình", Description = "Xe đạp đua đường trường, xe đạp leo núi dã ngoại", IsActive = true },
                    new VehicleCategory { Name = "Xe mô tô phân khối lớn", Description = "Mô tô thể thao, naked-bike, phong cách", IsActive = true },
                    new VehicleCategory { Name = "Ô tô SUV 7 chỗ gia đình", Description = "Xe gầm cao 7 chỗ rộng rãi, tiện lợi đi xa", IsActive = true },
                    new VehicleCategory { Name = "Siêu xe & Xe sự kiện", Description = "Dòng xe sang trọng phục vụ quay phim, chụp ảnh cưới và sự kiện", IsActive = true },
                    new VehicleCategory { Name = "Xe bán tải / Bán tải địa hình", Description = "Dòng xe bán tải chở đồ nội khu, dã ngoại, di chuyển đa địa hình", IsActive = true },
                    new VehicleCategory { Name = "Xe máy côn tay thể thao", Description = "Xe tay côn phân khối 150cc cá tính cho các bạn trẻ", IsActive = true }
                };

                foreach (var cat in categoriesToAdd)
                {
                    if (!existingCategoryNames.Contains(cat.Name))
                    {
                        context.VehicleCategories.Add(cat);
                    }
                }
                context.SaveChanges();

                if (!context.Roles.Any())
                {
                    var roleNames = new[] { "Admin", "Resident", "VehicleOwner" };
                    foreach (var rName in roleNames)
                    {
                        context.Roles.Add(new IdentityRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = rName,
                            NormalizedName = rName.ToUpper()
                        });
                    }
                    context.SaveChanges();
                }

                var allCommunities = context.Communities.ToList();
                Community GetCommunityByIdx(int idx) => allCommunities[idx % allCommunities.Count];

                var existingUsers = context.Users.Select(u => u.UserName).ToHashSet();
                var passwordHasher = new PasswordHasher<User>();

                var sampleUsers = new List<(string email, string fullName, int communityIdx, string phone, MemberStatus status)>
                {
                    ("cudan_a@neighborride.vn", "Nguyễn Văn Cư Dân", 0, "0912345671", MemberStatus.Active),
                    ("tran_mai@neighborride.vn", "Trần Thị Mai", 1, "0912345672", MemberStatus.Active),
                    ("hoang_long@neighborride.vn", "Lê Hoàng Long", 2, "0912345673", MemberStatus.Active),
                    ("duc_anh@neighborride.vn", "Phạm Đức Anh", 3, "0912345674", MemberStatus.Active),
                    ("minh_chau@neighborride.vn", "Hoàng Minh Châu", 4, "0912345675", MemberStatus.Active),
                    ("thu_huong@neighborride.vn", "Nguyễn Thu Hương", 5, "0912345676", MemberStatus.Active),
                    ("quoc_tuan@neighborride.vn", "Đặng Quốc Tuấn", 6, "0912345677", MemberStatus.Active),
                    ("thu_trang@neighborride.vn", "Vũ Thu Trang", 7, "0912345678", MemberStatus.Active),
                    ("minh_quang@neighborride.vn", "Bùi Minh Quang", 8, "0912345679", MemberStatus.Active),
                    ("phuong_thao@neighborride.vn", "Đoàn Phương Thảo", 9, "0912345680", MemberStatus.Active),
                    ("van_thanh@neighborride.vn", "Phan Văn Thành", 10, "0912345681", MemberStatus.Active),
                    ("thuy_linh@neighborride.vn", "Dương Thùy Linh", 11, "0912345682", MemberStatus.Active),
                    ("quoc_viet@neighborride.vn", "Ngô Quốc Việt", 0, "0912345683", MemberStatus.Active),
                    ("thuy_hang@neighborride.vn", "Đinh Thúy Hằng", 1, "0912345684", MemberStatus.Active),
                    ("duc_trong@neighborride.vn", "Hà Đức Trọng", 2, "0912345685", MemberStatus.Active),
                    ("bich_ngoc@neighborride.vn", "Lưu Bích Ngọc", 3, "0912345686", MemberStatus.Active),
                    ("hoai_an@neighborride.vn", "Trịnh Hoài An", 4, "0912345687", MemberStatus.Pending),
                    ("nam_khanh@neighborride.vn", "Võ Nam Khánh", 5, "0912345688", MemberStatus.Suspended)
                };

                foreach (var (email, fullName, communityIdx, phone, status) in sampleUsers)
                {
                    if (!existingUsers.Contains(email))
                    {
                        var user = new User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = email,
                            NormalizedUserName = email.ToUpper(),
                            Email = email,
                            NormalizedEmail = email.ToUpper(),
                            EmailConfirmed = true,
                            PhoneNumber = phone,
                            PhoneNumberConfirmed = true,
                            FullName = fullName,
                            CommunityId = GetCommunityByIdx(communityIdx).Id,
                            MemberStatus = status,
                            SecurityStamp = Guid.NewGuid().ToString("D"),
                            ConcurrencyStamp = Guid.NewGuid().ToString("D")
                        };
                        user.PasswordHash = passwordHasher.HashPassword(user, "ShareRide@2026");
                        context.Users.Add(user);
                    }
                }
                context.SaveChanges();

                var allUsers = context.Users.ToList();
                var allCategories = context.VehicleCategories.ToList();

                VehicleCategory GetCat(string name) => allCategories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) 
                                                        ?? allCategories.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) 
                                                        ?? allCategories[0];

                Community GetCom(string name) => allCommunities.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) 
                                                  ?? allCommunities[0];

                User GetUser(int idx) => allUsers[idx % allUsers.Count];

                if (context.Vehicles.Count() < 25)
                {
                    if (!context.Bookings.Any())
                    {
                        context.VehicleImages.RemoveRange(context.VehicleImages);
                        context.Vehicles.RemoveRange(context.Vehicles);
                        context.SaveChanges();
                    }

                    var catCar = GetCat("Ô tô");
                    var catElectricCar = GetCat("Ô tô điện");
                    var catMotorbike = GetCat("Xe máy");
                    var catElectricBike = GetCat("Xe máy điện");
                    var catBicycle = GetCat("Xe đạp");

                    var comOceanPark = GetCom("Ocean Park");
                    var comEcoPark = GetCom("EcoPark");
                    var comGreenStars = GetCom("Green Stars");
                    var comGreenBay = GetCom("Green Bay");
                    var comMatrixOne = GetCom("Matrix One");
                    var comSmartCity = GetCom("Smart City");
                    var comEcoHome = GetCom("EcoHome");

                    var vehicleSeedList = new List<(Vehicle vehicle, string imageUrl)>
                    {
                        (new Vehicle {
                            Name = "Ferrari 488 Spider Verde",
                            Description = "Siêu xe mui trần động cơ V8 Twin-Turbo 670 mã lực, màu xanh cốm cực hiếm. Phù hợp quay MV, chụp ảnh cưới và trải nghiệm tốc độ.",
                            Condition = "Mới 99%, xe lướt, bảo hiểm thân vỏ đầy đủ",
                            ShareType = ShareType.Rental, Price = 4500000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(0).Id
                        }, "/images/vehicles/ferrari-488.png"),

                        (new Vehicle {
                            Name = "McLaren 720S Performance",
                            Description = "Siêu phẩm cửa cánh bướm màu xanh Curacao Blue độc nhất, mui kính Panorama, nội thất Alcantara thể thao.",
                            Condition = "Mới 99%, vừa bảo dưỡng tại hãng",
                            ShareType = ShareType.Rental, Price = 5000000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(1).Id
                        }, "/images/vehicles/mclaren-720s.png"),

                        (new Vehicle {
                            Name = "Ferrari LaFerrari Hypercar",
                            Description = "Huyền thoại Hypercar động cơ Hybrid V12 công suất gần 1000 mã lực. Xe trưng bày sự kiện, quay chụp VIP tại nội khu đô thị.",
                            Condition = "Mới 99%, bảo quản gara máy lạnh",
                            ShareType = ShareType.Rental, Price = 6500000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(2).Id
                        }, "/images/vehicles/ferrari-laferrari.png"),

                        (new Vehicle {
                            Name = "Toyota GT86 Widebody Thể Thao",
                            Description = "Xe thể thao 2 cửa cầu sau drift cực mượt, gói độ bodykit thể thao, pô nổ trầm ấm không ồn ào.",
                            Condition = "Mới 96%, số tự động dễ lái",
                            ShareType = ShareType.Rental, Price = 1200000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(3).Id
                        }, "/images/vehicles/toyota-gt86.png"),

                        (new Vehicle {
                            Name = "Toyota Vios 2022 1.5G",
                            Description = "Xe số tự động đời mới, nội thất sạch sẽ, camera hành trình và cảm biến lùi. Thích hợp đi làm, về quê cuối tuần.",
                            Condition = "Mới 98%, bảo dưỡng định kỳ hãng",
                            ShareType = ShareType.Rental, Price = 700000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(4).Id
                        }, "/images/vehicles/toyota-vios.jpg"),

                        (new Vehicle {
                            Name = "Mazda CX-5 2.0 Premium 2021",
                            Description = "SUV 5 chỗ gầm cao, cửa sổ trời, âm thanh Bose cao cấp, phù hợp gia đình đi dã ngoại hoặc công tác ngoại tỉnh.",
                            Condition = "Mới 96%, xe gia đình không mùi thuốc",
                            ShareType = ShareType.Rental, Price = 900000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(5).Id
                        }, "/images/vehicles/mazda-cx5.jpg"),

                        (new Vehicle {
                            Name = "Hyundai Elantra 2.0 GLS Trắng",
                            Description = "Sedan hạng C phong cách trẻ trung, ghế da cao cấp chỉnh điện, sạc không dây, đi đầm chắc và cách âm tốt.",
                            Condition = "Mới 97%, xe đi giữ gìn",
                            ShareType = ShareType.Rental, Price = 750000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comEcoHome.Id, OwnerId = GetUser(6).Id
                        }, "/images/vehicles/hyundai-elantra.png"),

                        (new Vehicle {
                            Name = "Toyota Vios 2023 Màu Bạc",
                            Description = "Đời mới 2023 có màn hình Android lớn, cảnh báo tốc độ Vietmap, ghế da sạch sẽ, điều hòa mát rượi.",
                            Condition = "Mới 98%, lăn bánh 15.000km",
                            ShareType = ShareType.Rental, Price = 720000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(7).Id
                        }, "/images/vehicles/toyota-vios.jpg"),

                        (new Vehicle {
                            Name = "Mazda CX-5 2.5 AWD Signature",
                            Description = "Bản cao cấp 2 cầu AWD leo dốc mượt, có màn hình HUD hiển thị kính lái, camera 360 độ an toàn.",
                            Condition = "Mới 97%, lốp mới thay êm ái",
                            ShareType = ShareType.Rental, Price = 950000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(8).Id
                        }, "/images/vehicles/mazda-cx5.jpg"),

                        (new Vehicle {
                            Name = "Hyundai Elantra Sport Turbo",
                            Description = "Động cơ tăng áp 1.6 Turbo 204 mã lực cực bốc, lẫy chuyển số thể thao, xe chạy chắc nịch trên cao tốc.",
                            Condition = "Mới 95%, bảo dưỡng đầy đủ",
                            ShareType = ShareType.Rental, Price = 800000, Status = VehicleStatus.Available,
                            CategoryId = catCar.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(9).Id
                        }, "/images/vehicles/hyundai-elantra.png"),

                        (new Vehicle {
                            Name = "VinFast VF e34 Xanh SM",
                            Description = "Xe điện thông minh trợ lý ảo ViVi, không tiếng ồn động cơ, có trạm sạc nhanh dưới hầm chung cư.",
                            Condition = "Mới 98%, pin khỏe đi 280km/lần sạc",
                            ShareType = ShareType.Rental, Price = 650000, Status = VehicleStatus.Available,
                            CategoryId = catElectricCar.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(10).Id
                        }, "/images/vehicles/vinfast-vfe34.png"),

                        (new Vehicle {
                            Name = "VinFast VF e34 Xanh Ngọc",
                            Description = "Màu xanh ngọc dịu mát, khoang xe rộng rãi, có kèm cáp sạc tại nhà cho cư dân sử dụng.",
                            Condition = "Mới 96%, sạch sẽ không mùi",
                            ShareType = ShareType.Rental, Price = 600000, Status = VehicleStatus.Available,
                            CategoryId = catElectricCar.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(11).Id
                        }, "/images/vehicles/vinfast-vfe34.png"),

                        (new Vehicle {
                            Name = "VinFast VF e34 Plus Đô Thị",
                            Description = "Bản trang bị cao cấp, camera 360 độ hỗ trợ lùi chuồng hẹp cực chuẩn, cốp điện tiện ích.",
                            Condition = "Mới 97%, xe gia đình đi lại",
                            ShareType = ShareType.Rental, Price = 680000, Status = VehicleStatus.Available,
                            CategoryId = catElectricCar.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(12).Id
                        }, "/images/vehicles/vinfast-vfe34.png"),

                        (new Vehicle {
                            Name = "Honda Vision 2023 Đỏ Candy",
                            Description = "Xe tay ga tiết kiệm xăng số 1, cốp rộng, có khóa thông minh Smartkey, di chuyển nội khu cực kỳ nhẹ nhàng.",
                            Condition = "Mới 95%, máy êm ru",
                            ShareType = ShareType.Rental, Price = 120000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(13).Id
                        }, "/images/vehicles/honda-vision.jpg"),

                        (new Vehicle {
                            Name = "Yamaha NVX 155 VVA Thể Thao",
                            Description = "Xe tay ga thể thao 155cc, phanh ABS an toàn, khóa thông minh, lốp không săm bám đường tốt.",
                            Condition = "Mới 99%, xe chạy lướt",
                            ShareType = ShareType.Rental, Price = 150000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(14).Id
                        }, "/images/vehicles/yamaha-nvx.jpg"),

                        (new Vehicle {
                            Name = "Yamaha Janus 125 Xám Nhám",
                            Description = "Thiết kế thời trang châu Âu, động cơ BlueCore êm ái, xe nhẹ chỉ 97kg nữ dắt xe dễ dàng.",
                            Condition = "Mới 97%, khóa Smartkey tiện lợi",
                            ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comEcoHome.Id, OwnerId = GetUser(15).Id
                        }, "/images/vehicles/yamaha-janus.png"),

                        (new Vehicle {
                            Name = "Honda Vision Trắng Sữa 2022",
                            Description = "Xe màu trắng sữa trang nhã, đã dán keo chống xước, gương zin và kèm 2 mũ bảo hiểm.",
                            Condition = "Mới 96%, bảo dưỡng định kỳ",
                            ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(0).Id
                        }, "/images/vehicles/honda-vision.jpg"),

                        (new Vehicle {
                            Name = "Yamaha NVX 2023 Xanh GP",
                            Description = "Bản giới hạn thể thao, giảm xóc bình dầu êm ái, cổng sạc điện thoại tiện lợi trên đầu xe.",
                            Condition = "Mới 98%, xe chạy đầm",
                            ShareType = ShareType.Rental, Price = 160000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(1).Id
                        }, "/images/vehicles/yamaha-nvx.jpg"),

                        (new Vehicle {
                            Name = "Yamaha Janus 2023 Bản Đặc Biệt",
                            Description = "Màu đen mờ cá tính, yên xe thêu chỉ đỏ, công nghệ ngắt động cơ tạm thời tiết kiệm xăng.",
                            Condition = "Mới 98%, xe đi rất ít",
                            ShareType = ShareType.Rental, Price = 120000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(2).Id
                        }, "/images/vehicles/yamaha-janus.png"),

                        (new Vehicle {
                            Name = "Honda Vision 2024 Bản Cá Tính",
                            Description = "Vành đúc 16 inch thể thao, tư thế ngồi thoải mái, xe đăng ký đầu năm 2024 mới tinh.",
                            Condition = "Mới 99%, lăn bánh 3000km",
                            ShareType = ShareType.Rental, Price = 130000, Status = VehicleStatus.Available,
                            CategoryId = catMotorbike.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(3).Id
                        }, "/images/vehicles/honda-vision.jpg"),

                        // Xe máy điện
                        (new Vehicle {
                            Name = "Honda Cub e: Hồng Pastel",
                            Description = "Thiết kế retro cổ điển cực xinh, không cần bằng lái, pin sạc Lithium tháo rời mang lên căn hộ sạc được.",
                            Condition = "Mới 99%, xe đi nhẹ nhàng nội khu",
                            ShareType = ShareType.Rental, Price = 90000, Status = VehicleStatus.Available,
                            CategoryId = catElectricBike.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(4).Id
                        }, "/images/vehicles/honda-cube.png"),

                        (new Vehicle {
                            Name = "Honda Electric CB Thể Thao",
                            Description = "Xe mô tô điện kiểu dáng thể thao, động cơ điện mạnh mẽ, vận hành êm không tiếng ồn, đi 80km/lần sạc.",
                            Condition = "Mới 98%, sạc đầy sẵn sàng",
                            ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                            CategoryId = catElectricBike.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(5).Id
                        }, "/images/vehicles/honda-electric.png"),

                        (new Vehicle {
                            Name = "Honda Cub e: Trắng Kem Nội Khu",
                            Description = "Gọn gàng xinh xắn, có bàn đạp trợ lực khi hết pin, đèn pha LED tròn sáng rõ buổi tối.",
                            Condition = "Mới 97%, phù hợp đi chợ, đón bé",
                            ShareType = ShareType.Rental, Price = 80000, Status = VehicleStatus.Available,
                            CategoryId = catElectricBike.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(6).Id
                        }, "/images/vehicles/honda-cube.png"),

                        (new Vehicle {
                            Name = "Honda Electric Đô Thị Xanh",
                            Description = "Phương tiện đi làm và đi dạo nội khu bảo vệ môi trường, phanh đĩa an toàn, đồng hồ LCD sắc nét.",
                            Condition = "Mới 96%, kèm 2 chìa khóa và sạc",
                            ShareType = ShareType.Rental, Price = 100000, Status = VehicleStatus.Available,
                            CategoryId = catElectricBike.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(7).Id
                        }, "/images/vehicles/honda-electric.png"),

                        (new Vehicle {
                            Name = "Honda Cub e: Xanh Mint Cổ Điển",
                            Description = "Màu xanh mint bắt mắt, xe chạy êm ru, lốp không săm an toàn không lo xẹp bánh.",
                            Condition = "Mới 98%, xe chạy lướt",
                            ShareType = ShareType.Rental, Price = 95000, Status = VehicleStatus.Available,
                            CategoryId = catElectricBike.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(8).Id
                        }, "/images/vehicles/honda-cube.png"),

                        (new Vehicle {
                            Name = "Xe đạp thể thao Giant Escape 2",
                            Description = "Xe đạp thể thao khung nhôm ALUXX siêu nhẹ, bộ truyền động Shimano 21 tốc độ. Chia sẻ miễn phí cho cư dân rèn luyện sức khỏe.",
                            Condition = "Rất tốt, xích líp tra dầu định kỳ",
                            ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                            CategoryId = catBicycle.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(9).Id
                        }, "https://images.unsplash.com/photo-1485965120184-e220f721d03e?w=600&auto=format&fit=crop&q=80"),

                        (new Vehicle {
                            Name = "Xe đạp địa hình Trek Marlin 5",
                            Description = "Xe leo dốc địa hình bánh 29 inch, phuộc dầu êm ái, phanh đĩa dầu Tektro cao cấp, phù hợp đi phượt dã ngoại EcoPark.",
                            Condition = "Mới 95%, lốp địa hình gai dày",
                            ShareType = ShareType.Rental, Price = 50000, Status = VehicleStatus.Available,
                            CategoryId = catBicycle.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(10).Id
                        }, "https://images.unsplash.com/photo-1532298229144-0ec0c57515c7?w=600&auto=format&fit=crop&q=80"),

                        (new Vehicle {
                            Name = "Xe đạp dạo phố Asama City Life",
                            Description = "Xe đạp mini nữ có giỏ trước lớn, yên bọc da êm, có gác-ba-ga đèo em bé. Miễn phí cho cư dân trong khu vực.",
                            Condition = "Tốt, phanh chắc chắn, có chuông reo",
                            ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                            CategoryId = catBicycle.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(11).Id
                        }, "https://images.unsplash.com/photo-1507035895480-2b3156c31fc8?w=600&auto=format&fit=crop&q=80"),

                        (new Vehicle {
                            Name = "Xe đạp thể thao đường trường Twitter R10",
                            Description = "Xe đạp đua khung Carbon siêu nhẹ chỉ 8.5kg, tay lắc Shimano Claris, lốp xe đua tốc độ cao.",
                            Condition = "Mới 98%, dành cho người yêu đạp xe",
                            ShareType = ShareType.Rental, Price = 60000, Status = VehicleStatus.Available,
                            CategoryId = catBicycle.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(12).Id
                        }, "https://images.unsplash.com/photo-1576435728678-68d0fbf94e91?w=600&auto=format&fit=crop&q=80"),

                        (new Vehicle {
                            Name = "Xe đạp đôi dạo hồ EcoPark",
                            Description = "Xe đạp đôi 2 chỗ ngồi phù hợp cặp đôi và gia đình nhỏ dạo quanh công viên hồ Thiên Nga cuối tuần. Miễn phí cư dân.",
                            Condition = "Mới 95%, 2 phanh trước sau an toàn",
                            ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                            CategoryId = catBicycle.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(13).Id
                        }, "https://images.unsplash.com/photo-1528629297340-d1d461b55f91?w=600&auto=format&fit=crop&q=80")
                    };

                    foreach (var item in vehicleSeedList)
                    {
                        context.Vehicles.Add(item.vehicle);
                    }
                    context.SaveChanges();

                    foreach (var item in vehicleSeedList)
                    {
                        context.VehicleImages.Add(new VehicleImage
                        {
                            VehicleId = item.vehicle.Id,
                            ImageUrl = item.imageUrl,
                            IsPrimary = true
                        });
                    }
                    context.SaveChanges();
                }

                if (context.VehicleImages.Count() < 35)
                {
                    var vehiclesList = context.Vehicles.ToList();
                    var secondaryImages = new List<(int vehicleIdx, string imgUrl)>
                    {
                        (0, "/images/vehicles/ferrari-488.png"),
                        (1, "/images/vehicles/mclaren-720s.png"),
                        (4, "/images/vehicles/toyota-vios.jpg"),
                        (5, "/images/vehicles/mazda-cx5.jpg"),
                        (6, "/images/vehicles/hyundai-elantra.png"),
                        (10, "/images/vehicles/vinfast-vfe34.png"),
                        (13, "/images/vehicles/honda-vision.jpg"),
                        (14, "/images/vehicles/yamaha-nvx.jpg")
                    };

                    foreach (var (idx, url) in secondaryImages)
                    {
                        if (idx < vehiclesList.Count)
                        {
                            context.VehicleImages.Add(new VehicleImage
                            {
                                VehicleId = vehiclesList[idx].Id,
                                ImageUrl = url,
                                IsPrimary = false
                            });
                        }
                    }
                    context.SaveChanges();
                }

                if (!context.VehicleAvailabilities.Any())
                {
                    var vehicles = context.Vehicles.ToList();
                    var availabilities = new List<VehicleAvailability>();
                    var now = DateTime.UtcNow.Date;

                    for (int i = 0; i < Math.Min(25, vehicles.Count); i++)
                    {
                        var v = vehicles[i];
                        availabilities.Add(new VehicleAvailability
                        {
                            VehicleId = v.Id,
                            StartAt = now.AddDays(1 + (i % 3)).AddHours(7),
                            EndAt = now.AddDays(7 + (i % 7)).AddHours(21)
                        });
                    }

                    context.VehicleAvailabilities.AddRange(availabilities);
                    context.SaveChanges();
                }

                if (!context.Bookings.Any())
                {
                    var vehicles = context.Vehicles.ToList();
                    var users = context.Users.ToList();
                    var now = DateTime.UtcNow;

                    var bookingTemplates = new List<(int vehicleIdx, int userIdx, int startOffsetDays, int durationDays, BookingStatus status)>
                    {
                        (0, 1, -12, 2, BookingStatus.Completed),
                        (1, 2, -10, 1, BookingStatus.Completed),
                        (4, 3, -9, 3, BookingStatus.Completed),
                        (5, 4, -8, 2, BookingStatus.Completed),
                        (6, 5, -7, 1, BookingStatus.Completed),
                        (10, 6, -6, 2, BookingStatus.Completed),
                        (13, 7, -5, 1, BookingStatus.Completed),
                        (14, 8, -4, 2, BookingStatus.Completed),
                        (15, 9, -4, 1, BookingStatus.Completed),
                        (20, 10, -3, 1, BookingStatus.Completed),
                        (25, 11, -3, 2, BookingStatus.Completed),
                        (26, 12, -2, 1, BookingStatus.Completed),

                        (4, 2, -1, 2, BookingStatus.Ongoing),
                        (10, 3, 0, 2, BookingStatus.Ongoing),
                        (13, 4, 0, 1, BookingStatus.Ongoing),
                        (21, 5, 0, 1, BookingStatus.Ongoing),

                        (5, 6, 2, 3, BookingStatus.Confirmed),
                        (7, 7, 3, 2, BookingStatus.Confirmed),
                        (14, 8, 2, 1, BookingStatus.Confirmed),
                        (22, 9, 4, 2, BookingStatus.Confirmed),

                        (0, 10, 5, 1, BookingStatus.Pending),
                        (6, 11, 4, 2, BookingStatus.Pending),
                        (16, 12, 3, 1, BookingStatus.Pending),

                        (8, 13, -15, 2, BookingStatus.Cancelled),
                        (9, 14, 2, 1, BookingStatus.Rejected)
                    };

                    var bookings = new List<Booking>();
                    foreach (var (vIdx, uIdx, startOffsetDays, durationDays, status) in bookingTemplates)
                    {
                        var vehicle = vehicles[vIdx % vehicles.Count];
                        var borrower = users[uIdx % users.Count];

                        if (borrower.Id == vehicle.OwnerId)
                        {
                            borrower = users[(uIdx + 1) % users.Count];
                        }

                        var startAt = now.AddDays(startOffsetDays).Date.AddHours(8);
                        var endAt = startAt.AddDays(durationDays).Date.AddHours(18);
                        var estimatedFee = vehicle.ShareType == ShareType.Free ? 0 : vehicle.Price * durationDays;

                        bookings.Add(new Booking
                        {
                            VehicleId = vehicle.Id,
                            BorrowerId = borrower.Id,
                            StartAt = startAt,
                            EndAt = endAt,
                            EstimatedFee = estimatedFee,
                            Status = status,
                            CreatedAt = startAt.AddDays(-2)
                        });
                    }

                    context.Bookings.AddRange(bookings);
                    context.SaveChanges();
                }

                if (!context.HandoverRecords.Any())
                {
                    var completedOrOngoingBookings = context.Bookings
                        .Where(b => b.Status == BookingStatus.Completed || b.Status == BookingStatus.Ongoing)
                        .ToList();

                    var handovers = new List<HandoverRecord>();
                    foreach (var bk in completedOrOngoingBookings)
                    {
                        if (bk.Status == BookingStatus.Completed)
                        {
                            handovers.Add(new HandoverRecord
                            {
                                BookingId = bk.Id,
                                HandedOverAt = bk.StartAt,
                                ReturnedAt = bk.EndAt,
                                ConditionBefore = "Xe đã được vệ sinh sạch sẽ, nhiên liệu/pin đầy đủ 100%, có trầy nhẹ ở cản trước đã ghi nhận cùng chủ xe.",
                                ConditionAfter = "Người thuê hoàn trả đúng hạn, xe nguyên vẹn không trầy xước phát sinh, đã đổ đầy nhiên liệu như ban đầu."
                            });
                        }
                        else if (bk.Status == BookingStatus.Ongoing)
                        {
                            handovers.Add(new HandoverRecord
                            {
                                BookingId = bk.Id,
                                HandedOverAt = bk.StartAt,
                                ReturnedAt = null,
                                ConditionBefore = "Bàn giao xe kèm 2 chìa khóa và giấy tờ lưu hành nội khu. Kiểm tra lốp và phanh an toàn tuyệt đối.",
                                ConditionAfter = null
                            });
                        }
                    }

                    context.HandoverRecords.AddRange(handovers);
                    context.SaveChanges();
                }

                if (!context.Reviews.Any())
                {
                    var completedBookings = context.Bookings
                        .Where(b => b.Status == BookingStatus.Completed)
                        .Select(b => new { b.Id, b.BorrowerId, OwnerId = b.Vehicle.OwnerId })
                        .ToList();

                    var reviews = new List<Review>();
                    var reviewerPairs = new HashSet<string>();

                    var borrowerComments = new[]
                    {
                        "Xe đi rất êm và tiết kiệm nhiên liệu, chủ xe hỗ trợ bàn giao nhiệt tình và đúng giờ!",
                        "Trải nghiệm tuyệt vời! Xe sạch bóng, máy lạnh mát rượi, thủ tục nhận xe nhanh gọn.",
                        "Phương tiện hoạt động tốt, phanh và lốp an toàn, rất hài lòng với chất lượng dịch vụ nội khu.",
                        "Xe mượt mà, chủ xe thân thiện hướng dẫn chi tiết cách sử dụng các tính năng thông minh.",
                        "Rất tiện lợi khi cần xe gấp trong nội khu. Giá cả hợp lý và minh bạch, sẽ tiếp tục ủng hộ!",
                        "Xe đẹp như hình chụp, cảm giác lái đầm chắc, 5 sao cho sự chu đáo của chủ xe."
                    };

                    var ownerComments = new[]
                    {
                        "Người thuê giữ gìn xe rất cẩn thận, trả đúng giờ và rửa xe sạch sẽ trước khi bàn giao.",
                        "Cư dân văn minh, giao tiếp lịch sự và tôn trọng tài sản chung. Rất vui được chia sẻ xe!",
                        "Tuyệt vời! Hoàn trả xe đúng hẹn, bình xăng đầy đủ như lúc nhận. Đánh giá 5 sao.",
                        "Khách thuê thân thiện, tuân thủ đúng cam kết và quy định của cộng đồng."
                    };

                    int cIdx = 0;
                    foreach (var bk in completedBookings)
                    {
                        var key1 = $"{bk.Id}_{bk.BorrowerId}";
                        if (!reviewerPairs.Contains(key1))
                        {
                            reviewerPairs.Add(key1);
                            reviews.Add(new Review
                            {
                                BookingId = bk.Id,
                                ReviewerId = bk.BorrowerId,
                                RevieweeId = bk.OwnerId,
                                Rating = 5 - (cIdx % 2 == 0 ? 0 : 1), // 5 hoặc 4 sao
                                Comment = borrowerComments[cIdx % borrowerComments.Length],
                                CreatedAt = DateTime.UtcNow.AddDays(-cIdx)
                            });
                        }

                        if (cIdx % 2 == 0)
                        {
                            var key2 = $"{bk.Id}_{bk.OwnerId}";
                            if (!reviewerPairs.Contains(key2))
                            {
                                reviewerPairs.Add(key2);
                                reviews.Add(new Review
                                {
                                    BookingId = bk.Id,
                                    ReviewerId = bk.OwnerId,
                                    RevieweeId = bk.BorrowerId,
                                    Rating = 5,
                                    Comment = ownerComments[(cIdx / 2) % ownerComments.Length],
                                    CreatedAt = DateTime.UtcNow.AddDays(-cIdx).AddHours(2)
                                });
                            }
                        }

                        cIdx++;
                    }

                    context.Reviews.AddRange(reviews);
                    context.SaveChanges();
                }

                if (!context.ViolationReports.Any())
                {
                    var users = context.Users.ToList();
                    var vehicles = context.Vehicles.ToList();
                    var bookings = context.Bookings.ToList();

                    var reportTemplates = new List<(int reporterIdx, ReportTargetType type, string targetId, string reason, string desc, ReportStatus status, string? adminNote)>
                    {
                        (0, ReportTargetType.Vehicle, vehicles[0].Id.ToString(), "Đỗ xe sai quy định", "Xe đỗ lấn chiếm làn đường nội bộ trước sảnh R1 gây cản trở các phương tiện khác.", ReportStatus.Resolved, "BQT đã nhắc nhở chủ xe và di dời vào bãi đỗ đúng vị trí."),
                        (1, ReportTargetType.Vehicle, vehicles[4].Id.ToString(), "Phương tiện chưa được vệ sinh", "Xe bàn giao còn vệt bùn đất bên ngoài và rác bên trong hộc để đồ cánh cửa.", ReportStatus.Resolved, "Chủ xe đã nhận phản hồi và thanh toán phí vệ sinh cho người thuê."),
                        (2, ReportTargetType.Vehicle, vehicles[13].Id.ToString(), "Lốp xe mòn nguy hiểm", "Lốp sau xe máy có dấu hiệu nứt và mòn gai, đề nghị chủ xe thay mới để đảm bảo an toàn.", ReportStatus.Investigating, "Đang kiểm tra tình trạng thực tế phương tiện tại bãi đỗ."),
                        (3, ReportTargetType.Vehicle, vehicles[20].Id.ToString(), "Hỏng đèn xi-nhan trái", "Đèn xi-nhan bên trái không sáng khi bật công tắc.", ReportStatus.Pending, null),
                        (4, ReportTargetType.User, users[2].Id, "Trả xe trễ hẹn nhiều giờ", "Người thuê trả xe muộn hơn 3 tiếng mà không gọi điện thông báo trước làm lỡ lịch hẹn của chủ xe.", ReportStatus.Resolved, "Đã trừ điểm uy tín thành viên và thu bổ sung phụ phí trả trễ."),
                        (5, ReportTargetType.User, users[3].Id, "Hút thuốc lá trong xe", "Người thuê hút thuốc lá điện tử trong khoang cabin để lại mùi hôi khó chịu.", ReportStatus.Resolved, "Đã xử lý phạt vi phạm điều khoản cộng đồng và khử mùi ozon."),
                        (6, ReportTargetType.User, users[4].Id, "Chạy quá tốc độ nội khu", "Lái xe với tốc độ trên 30km/h trong khu vực có nhiều trẻ em vui chơi.", ReportStatus.Investigating, "Đang trích xuất dữ liệu camera an ninh khu dân cư."),
                        (7, ReportTargetType.User, users[5].Id, "Thái độ ứng xử thiếu văn hóa", "Có lời lẽ thiếu lịch sự khi bàn giao chìa khóa phương tiện.", ReportStatus.Dismissed, "Hai bên đã tự hòa giải và xác nhận không có tranh chấp."),
                        (8, ReportTargetType.Booking, bookings.Count > 0 ? bookings[0].Id.ToString() : "1", "Hủy chuyến đột xuất sát giờ", "Chủ xe hủy chuyến chỉ 30 phút trước giờ hẹn bàn giao mà không có lý do chính đáng.", ReportStatus.Resolved, "Đã hoàn 100% tiền cọc và tặng voucher đền bù cho người thuê."),
                        (9, ReportTargetType.Booking, bookings.Count > 1 ? bookings[1].Id.ToString() : "2", "Tranh chấp mức nhiên liệu", "Không thống nhất được mức vạch xăng ban đầu giữa hai bên.", ReportStatus.Resolved, "Đã đối chiếu ảnh chụp biên bản bàn giao và phân định rõ ràng."),
                        (10, ReportTargetType.Booking, bookings.Count > 2 ? bookings[2].Id.ToString() : "3", "Yêu cầu phụ phí không rõ ràng", "Chủ xe yêu cầu thêm phụ phí ngoài hợp đồng không có trong thỏa thuận ban đầu.", ReportStatus.Investigating, "BQT đang yêu cầu các bên cung cấp tin nhắn trao đổi."),
                        (11, ReportTargetType.Vehicle, vehicles[25].Id.ToString(), "Khóa xích xe đạp bị hỏng", "Khóa số kèm theo xe đạp bị kẹt số không mở được.", ReportStatus.Pending, null)
                    };

                    var reports = new List<ViolationReport>();
                    foreach (var (rIdx, type, targetId, reason, desc, status, adminNote) in reportTemplates)
                    {
                        reports.Add(new ViolationReport
                        {
                            ReporterId = users[rIdx % users.Count].Id,
                            TargetType = type,
                            TargetId = targetId,
                            Reason = reason,
                            Description = desc,
                            Status = status,
                            AdminNote = adminNote
                        });
                    }

                    context.ViolationReports.AddRange(reports);
                    context.SaveChanges();
                }

                if (!context.Notifications.Any())
                {
                    var users = context.Users.ToList();
                    var notifTemplates = new List<(int userIdx, string content, string type, bool isRead, int hoursAgo)>
                    {
                        (0, "Chào mừng bạn đến với ShareRide - Nền tảng chia sẻ phương tiện văn minh nội khu!", "System", true, 240),
                        (0, "Yêu cầu đặt xe Toyota Vios 2022 của bạn đã được tiếp nhận và đang chờ xử lý.", "Booking", true, 72),
                        (0, "Chuyến đi với xe Toyota Vios 2022 của bạn đã hoàn tất thành công. Hãy để lại đánh giá nhé!", "Review", true, 48),
                        (0, "Bạn nhận được 1 đánh giá 5 sao từ cư dân Trần Thị Mai.", "Review", false, 12),
                        (1, "Chủ xe đã xác nhận yêu cầu thuê xe McLaren 720S của bạn.", "Booking", true, 96),
                        (1, "Nhắc nhở: Lịch nhận xe của bạn bắt đầu vào lúc 08:00 sáng mai.", "Reminder", true, 50),
                        (1, "Bàn giao xe đã hoàn tất. Chúc bạn có một hành trình an toàn và vui vẻ!", "Handover", true, 40),
                        (2, "Có yêu cầu thuê mới cho xe Ferrari LaFerrari của bạn từ cư dân Lê Hoàng Long.", "Booking", true, 120),
                        (2, "Thành viên đã thanh toán thành công phí thuê xe qua hệ thống.", "Payment", true, 110),
                        (2, "Báo cáo vi phạm của bạn về việc đỗ xe sai vị trí đã được BQT xử lý thành công.", "Report", false, 5),
                        (3, "Yêu cầu thuê xe Toyota GT86 của bạn đã được phê duyệt.", "Booking", true, 80),
                        (3, "Nhắc nhở: Vui lòng trả xe trước 18:00 chiều nay theo đúng hợp đồng.", "Reminder", false, 4),
                        (4, "Phương tiện Mazda CX-5 của bạn vừa được cập nhật lịch sẵn sàng cho thuê.", "Vehicle", true, 150),
                        (4, "Biên bản bàn giao số #104 đã được lưu trữ thành công trên hệ thống.", "Handover", true, 60),
                        (5, "Đánh giá mới: 'Xe sạch đẹp, anh chủ nhiệt tình hỗ trợ 5 sao!'", "Review", false, 8),
                        (6, "Tài khoản cư dân của bạn tại Vinhomes Smart City đã được kích hoạt thành công.", "Account", true, 300),
                        (6, "Cảnh báo an ninh: Trạm sạc hầm B2 sẽ tạm bảo trì vào tối nay từ 23h đến 04h.", "Community", true, 18),
                        (7, "Bạn đã hủy thành công yêu cầu đặt xe VinFast VF e34.", "Booking", true, 85),
                        (8, "Cập nhật chính sách: Quy định an toàn giao thông mới trong khuôn viên Green Stars.", "Community", false, 2),
                        (9, "Xe máy điện Honda Cub e: của bạn đã được người thuê trả về bãi an toàn.", "Handover", true, 30),
                        (10, "Yêu cầu thuê xe đạp thể thao Giant Escape đã được xác nhận miễn phí.", "Booking", true, 45),
                        (11, "Bạn có 1 thông báo phản hồi từ Ban Quản Trị về đơn kiến nghị bãi đỗ xe.", "Support", false, 1),
                        (12, "Chúc mừng! Điểm đánh giá uy tín thành viên của bạn đạt mức tối đa 5.0/5.0.", "Achievement", true, 24),
                        (13, "Lưu ý: Thời tiết mưa to, vui lòng kiểm tra kỹ phanh và đèn trước khi di chuyển.", "Safety", false, 6),
                        (14, "Đã hoàn tiền cọc 500.000 đ cho mã giao dịch #SR-8921.", "Payment", true, 15)
                    };

                    var notifications = new List<Notification>();
                    foreach (var (uIdx, content, type, isRead, hoursAgo) in notifTemplates)
                    {
                        notifications.Add(new Notification
                        {
                            UserId = users[uIdx % users.Count].Id,
                            Content = content,
                            Type = type,
                            IsRead = isRead,
                            CreatedAt = DateTime.UtcNow.AddHours(-hoursAgo)
                        });
                    }

                    context.Notifications.AddRange(notifications);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbInitializer Error] {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}