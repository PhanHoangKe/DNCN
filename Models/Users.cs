using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduFlex.Areas.Admin.Models;

namespace EduFlex.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        public string? Avatar { get; set; }

        [StringLength(500, ErrorMessage = "Tiểu sử không quá 500 ký tự")]
        public string? Bio { get; set; }

        public bool IsActive { get; set; } = true;

        public bool EmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vai trò hợp lệ")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Roles? Role { get; set; }
    }
}