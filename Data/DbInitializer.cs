using System;
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

                if (!context.Communities.Any())
                {
                    context.Communities.AddRange(
                        new Community { Name = "Vinhomes Ocean Park", Address = "Gia Lâm, Hà Nội", IsActive = true },
                        new Community { Name = "Khu đô thị EcoPark", Address = "Văn Giang, Hưng Yên", IsActive = true },
                        new Community { Name = "Chung cư Green Stars", Address = "234 Phạm Văn Đồng, Hà Nội", IsActive = true }
                    );
                    context.SaveChanges();
                }

                if (!context.VehicleCategories.Any())
                {
                    context.VehicleCategories.AddRange(
                        new VehicleCategory { Name = "Xe đạp", Description = "Xe đạp cơ, xe đạp thể thao", IsActive = true },
                        new VehicleCategory { Name = "Xe đạp điện / Xe máy điện", Description = "Phương tiện chạy điện thân thiện môi trường", IsActive = true },
                        new VehicleCategory { Name = "Xe máy", Description = "Xe tay ga, xe số các loại", IsActive = true },
                        new VehicleCategory { Name = "Ô tô điện", Description = "Xe ô tô điện di chuyển nội khu và lân cận", IsActive = true }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    var community = context.Communities.FirstOrDefault();
                    var demoUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "cudan_a@neighborride.vn",
                        NormalizedUserName = "CUDAN_A@NEIGHBORRIDE.VN",
                        Email = "cudan_a@neighborride.vn",
                        NormalizedEmail = "CUDAN_A@NEIGHBORRIDE.VN",
                        EmailConfirmed = true,
                        FullName = "Nguyễn Văn Cư Dân",
                        CommunityId = community?.Id,
                        MemberStatus = MemberStatus.Active
                    };
                    context.Users.Add(demoUser);
                    context.SaveChanges();
                }
            }
            catch
            {
            }
        }
    }
}