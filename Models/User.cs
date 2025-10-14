using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public string? Bio { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public bool? EmailVerified { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

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

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SystemAnnouncement> SystemAnnouncements { get; set; } = new List<SystemAnnouncement>();
}
