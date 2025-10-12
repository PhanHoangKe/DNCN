using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class CourseView
{
    public int ViewId { get; set; }

    public int CourseId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ViewedAt { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User? User { get; set; }
}
