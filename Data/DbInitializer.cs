using System;
using System.Collections.Generic;
using System.Linq;
using btl_webnc_G08.Models.Entities;
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

                // 1. SEED KHU DÂN CƯ / CỘNG ĐỒNG
                var existingCommunityNames = context.Communities.Select(c => c.Name).ToHashSet();
                var communitiesToAdd = new List<Community>
                {
                    new Community { Name = "Vinhomes Ocean Park", Address = "Gia Lâm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị EcoPark", Address = "Văn Giang, Hưng Yên", IsActive = true },
                    new Community { Name = "Chung cư Green Stars", Address = "234 Phạm Văn Đồng, Bắc Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư Green Bay", Address = "Mễ Trì, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư The Matrix One", Address = "Lê Quang Đạo, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Khu đô thị Vinhomes Smart City", Address = "Tây Mỗ, Nam Từ Liêm, Hà Nội", IsActive = true },
                    new Community { Name = "Chung cư EcoHome 3", Address = "Đông Ngạc, Bắc Từ Liêm, Hà Nội", IsActive = true }
                };

                foreach (var com in communitiesToAdd)
                {
                    if (!existingCommunityNames.Contains(com.Name))
                    {
                        context.Communities.Add(com);
                    }
                }
                context.SaveChanges();

                // 2. SEED DANH MỤC PHƯƠNG TIỆN
                var existingCategoryNames = context.VehicleCategories.Select(c => c.Name).ToHashSet();
                var categoriesToAdd = new List<VehicleCategory>
                {
                    new VehicleCategory { Name = "Xe đạp", Description = "Xe đạp cơ, xe đạp thể thao, dạo phố", IsActive = true },
                    new VehicleCategory { Name = "Xe đạp điện / Xe máy điện", Description = "Phương tiện chạy điện thân thiện môi trường", IsActive = true },
                    new VehicleCategory { Name = "Xe máy", Description = "Xe tay ga, xe số các loại", IsActive = true },
                    new VehicleCategory { Name = "Ô tô điện", Description = "Xe ô tô điện di chuyển nội khu và lân cận", IsActive = true },
                    new VehicleCategory { Name = "Ô tô", Description = "Xe ô tô 4-7 chỗ, sedan, SUV, siêu xe thể thao", IsActive = true }
                };

                foreach (var cat in categoriesToAdd)
                {
                    if (!existingCategoryNames.Contains(cat.Name))
                    {
                        context.VehicleCategories.Add(cat);
                    }
                }
                context.SaveChanges();

                // 3. SEED NGƯỜI DÙNG CƯ DÂN
                var defaultCommunity = context.Communities.FirstOrDefault();
                var existingUsers = context.Users.Select(u => u.UserName).ToHashSet();
                var sampleUsers = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "cudan_a@neighborride.vn",
                        NormalizedUserName = "CUDAN_A@NEIGHBORRIDE.VN",
                        Email = "cudan_a@neighborride.vn",
                        NormalizedEmail = "CUDAN_A@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Nguyễn Văn Cư Dân",
                        CommunityId = defaultCommunity?.Id,
                        MemberStatus = MemberStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "tran_mai@neighborride.vn",
                        NormalizedUserName = "TRAN_MAI@NEIGHBORRIDE.VN",
                        Email = "tran_mai@neighborride.vn",
                        NormalizedEmail = "TRAN_MAI@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Trần Thị Mai",
                        CommunityId = defaultCommunity?.Id,
                        MemberStatus = MemberStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "hoang_long@neighborride.vn",
                        NormalizedUserName = "HOANG_LONG@NEIGHBORRIDE.VN",
                        Email = "hoang_long@neighborride.vn",
                        NormalizedEmail = "HOANG_LONG@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Lê Hoàng Long",
                        CommunityId = defaultCommunity?.Id,
                        MemberStatus = MemberStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "duc_anh@neighborride.vn",
                        NormalizedUserName = "DUC_ANH@NEIGHBORRIDE.VN",
                        Email = "duc_anh@neighborride.vn",
                        NormalizedEmail = "DUC_ANH@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Phạm Đức Anh",
                        CommunityId = defaultCommunity?.Id,
                        MemberStatus = MemberStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "minh_chau@neighborride.vn",
                        NormalizedUserName = "MINH_CHAU@NEIGHBORRIDE.VN",
                        Email = "minh_chau@neighborride.vn",
                        NormalizedEmail = "MINH_CHAU@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Hoàng Minh Châu",
                        CommunityId = defaultCommunity?.Id,
                        MemberStatus = MemberStatus.Active
                    }
                };

                foreach (var u in sampleUsers)
                {
                    if (!existingUsers.Contains(u.UserName))
                    {
                        context.Users.Add(u);
                    }
                }
                context.SaveChanges();

                // 4. SEED 30 PHƯƠNG TIỆN MẪU ĐA DẠNG
                if (context.Vehicles.Count() < 25)
                {
                    // Xóa dữ liệu cũ nếu chưa có đặt xe để tạo lại chuẩn 30 xe
                    if (!context.Bookings.Any())
                    {
                        context.VehicleImages.RemoveRange(context.VehicleImages);
                        context.Vehicles.RemoveRange(context.Vehicles);
                        context.SaveChanges();
                    }

                    var allUsers = context.Users.ToList();
                    var allCategories = context.VehicleCategories.ToList();
                    var allCommunities = context.Communities.ToList();

                    if (allUsers.Any() && allCategories.Any() && allCommunities.Any())
                    {
                        // Hàm helper lấy category, community, user an toàn
                        VehicleCategory GetCat(string name) => allCategories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) 
                                                                ?? allCategories.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) 
                                                                ?? allCategories[0];

                        Community GetCom(string name) => allCommunities.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) 
                                                          ?? allCommunities[0];

                        User GetUser(int idx) => allUsers[idx % allUsers.Count];

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

                        // Danh sách 30 xe chi tiết
                        var vehicleSeedList = new List<(Vehicle vehicle, string imageUrl)>
                        {
                            // Nhóm 1: Siêu xe & Xe thể thao (Sử dụng ảnh user)
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

                            // Nhóm 2: Ô tô Sedan & SUV gia đình (Sử dụng ảnh user)
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
                                CategoryId = catCar.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(0).Id
                            }, "/images/vehicles/mazda-cx5.jpg"),

                            (new Vehicle {
                                Name = "Hyundai Elantra 2.0 GLS Trắng",
                                Description = "Sedan hạng C phong cách trẻ trung, ghế da cao cấp chỉnh điện, sạc không dây, đi đầm chắc và cách âm tốt.",
                                Condition = "Mới 97%, xe đi giữ gìn",
                                ShareType = ShareType.Rental, Price = 750000, Status = VehicleStatus.Available,
                                CategoryId = catCar.Id, CommunityId = comEcoHome.Id, OwnerId = GetUser(1).Id
                            }, "/images/vehicles/hyundai-elantra.png"),

                            (new Vehicle {
                                Name = "Toyota Vios 2023 Màu Bạc",
                                Description = "Đời mới 2023 có màn hình Android lớn, cảnh báo tốc độ Vietmap, ghế da sạch sẽ, điều hòa mát rượi.",
                                Condition = "Mới 98%, lăn bánh 15.000km",
                                ShareType = ShareType.Rental, Price = 720000, Status = VehicleStatus.Available,
                                CategoryId = catCar.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(2).Id
                            }, "/images/vehicles/toyota-vios.jpg"),

                            (new Vehicle {
                                Name = "Mazda CX-5 2.5 AWD Signature",
                                Description = "Bản cao cấp 2 cầu AWD leo dốc mượt, có màn hình HUD hiển thị kính lái, camera 360 độ an toàn.",
                                Condition = "Mới 97%, lốp mới thay êm ái",
                                ShareType = ShareType.Rental, Price = 950000, Status = VehicleStatus.Available,
                                CategoryId = catCar.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(3).Id
                            }, "/images/vehicles/mazda-cx5.jpg"),

                            (new Vehicle {
                                Name = "Hyundai Elantra Sport Turbo",
                                Description = "Động cơ tăng áp 1.6 Turbo 204 mã lực cực bốc, lẫy chuyển số thể thao, xe chạy chắc nịch trên cao tốc.",
                                Condition = "Mới 95%, bảo dưỡng đầy đủ",
                                ShareType = ShareType.Rental, Price = 800000, Status = VehicleStatus.Available,
                                CategoryId = catCar.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(4).Id
                            }, "/images/vehicles/hyundai-elantra.png"),

                            // Nhóm 3: Ô tô điện thông minh (Sử dụng ảnh user)
                            (new Vehicle {
                                Name = "VinFast VF e34 Xanh SM",
                                Description = "Xe điện thông minh trợ lý ảo ViVi, không tiếng ồn động cơ, có trạm sạc nhanh dưới hầm chung cư.",
                                Condition = "Mới 98%, pin khỏe đi 280km/lần sạc",
                                ShareType = ShareType.Rental, Price = 650000, Status = VehicleStatus.Available,
                                CategoryId = catElectricCar.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(0).Id
                            }, "/images/vehicles/vinfast-vfe34.png"),

                            (new Vehicle {
                                Name = "VinFast VF e34 Xanh Ngọc",
                                Description = "Màu xanh ngọc dịu mát, khoang xe rộng rãi, có kèm cáp sạc tại nhà cho cư dân sử dụng.",
                                Condition = "Mới 96%, sạch sẽ không mùi",
                                ShareType = ShareType.Rental, Price = 600000, Status = VehicleStatus.Available,
                                CategoryId = catElectricCar.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(1).Id
                            }, "/images/vehicles/vinfast-vfe34.png"),

                            (new Vehicle {
                                Name = "VinFast VF e34 Plus Đô Thị",
                                Description = "Bản trang bị cao cấp, camera 360 độ hỗ trợ lùi chuồng hẹp cực chuẩn, cốp điện tiện ích.",
                                Condition = "Mới 97%, xe gia đình đi lại",
                                ShareType = ShareType.Rental, Price = 680000, Status = VehicleStatus.Available,
                                CategoryId = catElectricCar.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(2).Id
                            }, "/images/vehicles/vinfast-vfe34.png"),

                            // Nhóm 4: Xe máy tay ga phổ biến (Sử dụng ảnh user)
                            (new Vehicle {
                                Name = "Honda Vision 2023 Đỏ Candy",
                                Description = "Xe tay ga tiết kiệm xăng số 1, cốp rộng, có khóa thông minh Smartkey, di chuyển nội khu cực kỳ nhẹ nhàng.",
                                Condition = "Mới 95%, máy êm ru",
                                ShareType = ShareType.Rental, Price = 120000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(3).Id
                            }, "/images/vehicles/honda-vision.jpg"),

                            (new Vehicle {
                                Name = "Yamaha NVX 155 VVA Thể Thao",
                                Description = "Xe tay ga thể thao 155cc, phanh ABS an toàn, khóa thông minh, lốp không săm bám đường tốt.",
                                Condition = "Mới 99%, xe chạy lướt",
                                ShareType = ShareType.Rental, Price = 150000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(4).Id
                            }, "/images/vehicles/yamaha-nvx.jpg"),

                            (new Vehicle {
                                Name = "Yamaha Janus 125 Xám Nhám",
                                Description = "Thiết kế thời trang châu Âu, động cơ BlueCore êm ái, xe nhẹ chỉ 97kg nữ dắt xe dễ dàng.",
                                Condition = "Mới 97%, khóa Smartkey tiện lợi",
                                ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comEcoHome.Id, OwnerId = GetUser(0).Id
                            }, "/images/vehicles/yamaha-janus.png"),

                            (new Vehicle {
                                Name = "Honda Vision Trắng Sữa 2022",
                                Description = "Xe màu trắng sữa trang nhã, đã dán keo chống xước, gương zin và kèm 2 mũ bảo hiểm.",
                                Condition = "Mới 96%, bảo dưỡng định kỳ",
                                ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(1).Id
                            }, "/images/vehicles/honda-vision.jpg"),

                            (new Vehicle {
                                Name = "Yamaha NVX 2023 Xanh GP",
                                Description = "Bản giới hạn thể thao, giảm xóc bình dầu êm ái, cổng sạc điện thoại tiện lợi trên đầu xe.",
                                Condition = "Mới 98%, xe chạy đầm",
                                ShareType = ShareType.Rental, Price = 160000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(2).Id
                            }, "/images/vehicles/yamaha-nvx.jpg"),

                            (new Vehicle {
                                Name = "Yamaha Janus 2023 Bản Đặc Biệt",
                                Description = "Màu đen mờ cá tính, yên xe thêu chỉ đỏ, công nghệ ngắt động cơ tạm thời tiết kiệm xăng.",
                                Condition = "Mới 98%, xe đi rất ít",
                                ShareType = ShareType.Rental, Price = 120000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(3).Id
                            }, "/images/vehicles/yamaha-janus.png"),

                            (new Vehicle {
                                Name = "Honda Vision 2024 Bản Cá Tính",
                                Description = "Vành đúc 16 inch thể thao, tư thế ngồi thoải mái, xe đăng ký đầu năm 2024 mới tinh.",
                                Condition = "Mới 99%, lăn bánh 3000km",
                                ShareType = ShareType.Rental, Price = 130000, Status = VehicleStatus.Available,
                                CategoryId = catMotorbike.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(4).Id
                            }, "/images/vehicles/honda-vision.jpg"),

                            // Nhóm 5: Xe máy điện & Xe đạp điện (Sử dụng ảnh user)
                            (new Vehicle {
                                Name = "Honda Cub e: Hồng Pastel",
                                Description = "Thiết kế retro cổ điển cực xinh, không cần bằng lái, pin sạc Lithium tháo rời mang lên căn hộ sạc được.",
                                Condition = "Mới 99%, xe đi nhẹ nhàng nội khu",
                                ShareType = ShareType.Rental, Price = 90000, Status = VehicleStatus.Available,
                                CategoryId = catElectricBike.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(0).Id
                            }, "/images/vehicles/honda-cube.png"),

                            (new Vehicle {
                                Name = "Honda Electric CB Thể Thao",
                                Description = "Xe mô tô điện kiểu dáng thể thao, động cơ điện mạnh mẽ, vận hành êm không tiếng ồn, đi 80km/lần sạc.",
                                Condition = "Mới 98%, sạc đầy sẵn sàng",
                                ShareType = ShareType.Rental, Price = 110000, Status = VehicleStatus.Available,
                                CategoryId = catElectricBike.Id, CommunityId = comGreenBay.Id, OwnerId = GetUser(1).Id
                            }, "/images/vehicles/honda-electric.png"),

                            (new Vehicle {
                                Name = "Honda Cub e: Trắng Kem Nội Khu",
                                Description = "Gọn gàng xinh xắn, có bàn đạp trợ lực khi hết pin, đèn pha LED tròn sáng rõ buổi tối.",
                                Condition = "Mới 97%, phù hợp đi chợ, đón bé",
                                ShareType = ShareType.Rental, Price = 80000, Status = VehicleStatus.Available,
                                CategoryId = catElectricBike.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(2).Id
                            }, "/images/vehicles/honda-cube.png"),

                            (new Vehicle {
                                Name = "Honda Electric Đô Thị Xanh",
                                Description = "Phương tiện đi làm và đi dạo nội khu bảo vệ môi trường, phanh đĩa an toàn, đồng hồ LCD sắc nét.",
                                Condition = "Mới 96%, kèm 2 chìa khóa và sạc",
                                ShareType = ShareType.Rental, Price = 100000, Status = VehicleStatus.Available,
                                CategoryId = catElectricBike.Id, CommunityId = comSmartCity.Id, OwnerId = GetUser(3).Id
                            }, "/images/vehicles/honda-electric.png"),

                            (new Vehicle {
                                Name = "Honda Cub e: Xanh Mint Cổ Điển",
                                Description = "Màu xanh mint bắt mắt, xe chạy êm ru, lốp không săm an toàn không lo xẹp bánh.",
                                Condition = "Mới 98%, xe chạy lướt",
                                ShareType = ShareType.Rental, Price = 95000, Status = VehicleStatus.Available,
                                CategoryId = catElectricBike.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(4).Id
                            }, "/images/vehicles/honda-cube.png"),

                            // Nhóm 6: Xe đạp cơ & Xe chia sẻ miễn phí cư dân
                            (new Vehicle {
                                Name = "Xe đạp thể thao Giant Escape 2",
                                Description = "Xe đạp thể thao khung nhôm ALUXX siêu nhẹ, bộ truyền động Shimano 21 tốc độ. Chia sẻ miễn phí cho cư dân rèn luyện sức khỏe.",
                                Condition = "Rất tốt, xích líp tra dầu định kỳ",
                                ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                                CategoryId = catBicycle.Id, CommunityId = comOceanPark.Id, OwnerId = GetUser(0).Id
                            }, "https://images.unsplash.com/photo-1485965120184-e220f721d03e?w=600&auto=format&fit=crop&q=80"),

                            (new Vehicle {
                                Name = "Xe đạp địa hình Trek Marlin 5",
                                Description = "Xe leo dốc địa hình bánh 29 inch, phuộc dầu êm ái, phanh đĩa dầu Tektro cao cấp, phù hợp đi phượt dã ngoại EcoPark.",
                                Condition = "Mới 95%, lốp địa hình gai dày",
                                ShareType = ShareType.Rental, Price = 50000, Status = VehicleStatus.Available,
                                CategoryId = catBicycle.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(1).Id
                            }, "https://images.unsplash.com/photo-1532298229144-0ec0c57515c7?w=600&auto=format&fit=crop&q=80"),

                            (new Vehicle {
                                Name = "Xe đạp dạo phố Asama City Life",
                                Description = "Xe đạp mini nữ có giỏ trước lớn, yên bọc da êm, có gác-ba-ga đèo em bé. Miễn phí cho cư dân trong khu vực.",
                                Condition = "Tốt, phanh chắc chắn, có chuông reo",
                                ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                                CategoryId = catBicycle.Id, CommunityId = comGreenStars.Id, OwnerId = GetUser(2).Id
                            }, "https://images.unsplash.com/photo-1507035895480-2b3156c31fc8?w=600&auto=format&fit=crop&q=80"),

                            (new Vehicle {
                                Name = "Xe đạp thể thao đường trường Twitter R10",
                                Description = "Xe đạp đua khung Carbon siêu nhẹ chỉ 8.5kg, tay lắc Shimano Claris, lốp xe đua tốc độ cao.",
                                Condition = "Mới 98%, dành cho người yêu đạp xe",
                                ShareType = ShareType.Rental, Price = 60000, Status = VehicleStatus.Available,
                                CategoryId = catBicycle.Id, CommunityId = comMatrixOne.Id, OwnerId = GetUser(3).Id
                            }, "https://images.unsplash.com/photo-1576435728678-68d0fbf94e91?w=600&auto=format&fit=crop&q=80"),

                            (new Vehicle {
                                Name = "Xe đạp đôi dạo hồ EcoPark",
                                Description = "Xe đạp đôi 2 chỗ ngồi phù hợp cặp đôi và gia đình nhỏ dạo quanh công viên hồ Thiên Nga cuối tuần. Miễn phí cư dân.",
                                Condition = "Mới 95%, 2 phanh trước sau an toàn",
                                ShareType = ShareType.Free, Price = 0, Status = VehicleStatus.Available,
                                CategoryId = catBicycle.Id, CommunityId = comEcoPark.Id, OwnerId = GetUser(4).Id
                            }, "https://images.unsplash.com/photo-1528629297340-d1d461b55f91?w=600&auto=format&fit=crop&q=80")
                        };

                        // Thêm vào database
                        foreach (var item in vehicleSeedList)
                        {
                            context.Vehicles.Add(item.vehicle);
                        }
                        context.SaveChanges();

                        // Thêm ảnh tương ứng
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
                }
            }
            catch
            {
            }
        }
    }
}