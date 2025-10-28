using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Tiêu đề khóa học là bắt buộc")]
    [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
    public string CourseTitle { get; set; } = null!;

    [Required(ErrorMessage = "Slug là bắt buộc")]
    [StringLength(200, ErrorMessage = "Slug không quá 200 ký tự")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ chứa chữ thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Mô tả ngắn không quá 500 ký tự")]
    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? PreviewVideoUrl { get; set; }

    public decimal? Price { get; set; }

    public bool IsFree { get; set; }

    [Range(0, 999999999, ErrorMessage = "Giá giảm phải lớn hơn hoặc bằng 0")]
    public decimal? DiscountPrice { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giảng viên")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn giảng viên hợp lệ")]
    public int InstructorId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục hợp lệ")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn cấp độ")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn cấp độ hợp lệ")]
    public int LevelId { get; set; }

    public string? Language { get; set; }

    public int? Duration { get; set; }

    public int? TotalLessons { get; set; }

    public bool IsPublished { get; set; }

    public bool IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public int? ViewCount { get; set; }

    public int? EnrollmentCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int? TotalRatings { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Users? ApprovedByNavigation { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Categories Categories { get; set; } = null!;

    public virtual ICollection<CourseObjective> CourseObjectives { get; set; } = new List<CourseObjective>();

    public virtual ICollection<CourseRequirement> CourseRequirements { get; set; } = new List<CourseRequirement>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<CourseView> CourseViews { get; set; } = new List<CourseView>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Users Instructor { get; set; } = null!;

    public virtual CourseLevel Level { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<QnA> QnAs { get; set; } = new List<QnA>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
