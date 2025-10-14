using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class CourseReview
{
    [Key]
    public int ReviewId { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string? ReviewText { get; set; }

    public bool? IsApproved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Users Users { get; set; } = null!;
}
