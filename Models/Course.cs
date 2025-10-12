using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseTitle { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? PreviewVideoUrl { get; set; }

    public decimal? Price { get; set; }

    public bool? IsFree { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int InstructorId { get; set; }

    public int CategoryId { get; set; }

    public int LevelId { get; set; }

    public string? Language { get; set; }

    public int? Duration { get; set; }

    public int? TotalLessons { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public int? ViewCount { get; set; }

    public int? EnrollmentCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int? TotalRatings { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<CourseObjective> CourseObjectives { get; set; } = new List<CourseObjective>();

    public virtual ICollection<CourseRequirement> CourseRequirements { get; set; } = new List<CourseRequirement>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<CourseView> CourseViews { get; set; } = new List<CourseView>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual User Instructor { get; set; } = null!;

    public virtual CourseLevel Level { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<QnA> QnAs { get; set; } = new List<QnA>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
