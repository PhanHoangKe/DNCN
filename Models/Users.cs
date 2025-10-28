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
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
        public string? FullName { get; set; }

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
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Course> CourseApprovedByNavigations { get; set; } = new List<Course>();

    public virtual ICollection<Course> CourseInstructors { get; set; } = new List<Course>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<CourseView> CourseViews { get; set; } = new List<CourseView>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<LessonComment> LessonComments { get; set; } = new List<LessonComment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public virtual ICollection<QnAanswer> QnAanswers { get; set; } = new List<QnAanswer>();

    public virtual ICollection<QnA> QnAs { get; set; } = new List<QnA>();

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();


    public virtual ICollection<SystemAnnouncement> SystemAnnouncements { get; set; } = new List<SystemAnnouncement>();
    }
}