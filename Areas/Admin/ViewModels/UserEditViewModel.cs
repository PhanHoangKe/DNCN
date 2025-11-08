using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduFlex.Areas.Admin.ViewModels
{
    public class UserEditViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Tiểu sử")]
        [StringLength(500)]
        public string? Bio { get; set; }

        [Display(Name = "Ảnh đại diện hiện tại")]
        public string? CurrentAvatar { get; set; }

        [Display(Name = "Thay ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }

        [Display(Name = "Mật khẩu mới (để trống nếu không đổi)")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [Display(Name = "Vai trò")]
        public int RoleId { get; set; }

        [Display(Name = "Kích hoạt tài khoản")]
        public bool IsActive { get; set; }
    }
}