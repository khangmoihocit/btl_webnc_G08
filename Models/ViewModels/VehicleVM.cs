using System.ComponentModel.DataAnnotations;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Models.ViewModels
{
    public class VehicleItemVM
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CommunityId { get; set; }
        public string CommunityName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public ShareType ShareType { get; set; }
        public string ShareTypeName => ShareType == ShareType.Free ? "Miễn phí" : "Cho thuê";
        public decimal Price { get; set; }
        public VehicleStatus Status { get; set; }
        public string StatusName => Status switch
        {
            VehicleStatus.PendingApproval => "Chờ duyệt",
            VehicleStatus.Available => "Sẵn sàng",
            VehicleStatus.Reserved => "Đã đặt trước",
            VehicleStatus.InUse => "Đang sử dụng",
            VehicleStatus.Maintenance => "Bảo trì",
            VehicleStatus.Inactive => "Ngừng hoạt động",
            _ => Status.ToString()
        };
        public string? ImageUrl { get; set; }
    }

    public class VehicleFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủ sở hữu")]
        public string OwnerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục không hợp lệ")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khu dân cư")]
        [Range(1, int.MaxValue, ErrorMessage = "Khu dân cư không hợp lệ")]
        public int CommunityId { get; set; }

        [Required(ErrorMessage = "Tên phương tiện không được để trống")]
        [MaxLength(150, ErrorMessage = "Tên phương tiện tối đa 150 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? Description { get; set; }

        [MaxLength(500, ErrorMessage = "Tình trạng xe tối đa 500 ký tự")]
        public string? Condition { get; set; }

        public ShareType ShareType { get; set; } = ShareType.Free;

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Giá không được là số âm")]
        public decimal Price { get; set; } = 0;

        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        [MaxLength(500, ErrorMessage = "Đường dẫn ảnh quá dài")]
        public string? ImageUrl { get; set; }
    }
}
