using System.ComponentModel.DataAnnotations;
namespace btl_webnc_G08.Models.ViewModels;
public class RegisterVM {
 [Required, StringLength(100, MinimumLength=2)] public string FullName {get;set;}="";
 [Required, EmailAddress, MaxLength(254)] public string Email {get;set;}="";
 [Required,RegularExpression(@"^0[0-9]{9}$",ErrorMessage="Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.")] public string Phone {get;set;}="";
 [Required, StringLength(128, MinimumLength=8), DataType(DataType.Password)] public string Password {get;set;}="";
 [Required, Compare("Password"), DataType(DataType.Password)] public string ConfirmPassword {get;set;}="";
 [Range(1,int.MaxValue)] public int CommunityId {get;set;}
}
public class LoginVM {
 [Required, EmailAddress] public string Email {get;set;}="";
 [Required, DataType(DataType.Password)] public string Password {get;set;}="";
}
public class ReviewInput {
 [Range(1,int.MaxValue)] public int BookingId {get;set;}
 [Range(1,5)] public int Rating {get;set;}
 [MaxLength(1000)] public string? Comment {get;set;}
}
public class ReplyInput { [Required,StringLength(500,MinimumLength=5)] public string Content {get;set;}=""; }
public class ReportInput {
 [Required] public btl_webnc_G08.Models.Enums.AppEnums.ReportTargetType TargetType {get;set;}
 [Required,MaxLength(450)] public string TargetId {get;set;}="";
 [Required,StringLength(200,MinimumLength=5)] public string Reason {get;set;}="";
 [Required,StringLength(1500,MinimumLength=10)] public string Description {get;set;}="";
}

public class ProfileVM { [Required,StringLength(100,MinimumLength=2)] public string FullName {get;set;}=""; [Required,RegularExpression(@"^0[0-9]{9}$",ErrorMessage="Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.")] public string Phone {get;set;}=""; }
