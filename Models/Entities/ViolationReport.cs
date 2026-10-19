using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static btl_webnc_G08.Models.Enums.AppEnums;

namespace btl_webnc_G08.Models.Entities
{
    public class ViolationReport
    {
        public int Id { get; set; }
        public string ReporterId { get; set; } = string.Empty;
        public ReportTargetType TargetType { get; set; }
        public string TargetId { get; set; } = string.Empty;
        [MaxLength(200)] public string Reason { get; set; } = string.Empty;
        [MaxLength(1500)] public string Description { get; set; } = string.Empty;
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        [MaxLength(1000)] public string? AdminNote { get; set; }

        public virtual User Reporter { get; set; } = null!;
    }
}