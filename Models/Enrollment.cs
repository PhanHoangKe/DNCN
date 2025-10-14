using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EduFlex.Models;

public partial class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public decimal? Progress { get; set; }

    public DateTime? LastAccessedAt { get; set; }

    public bool? IsCertificateIssued { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual Users Users { get; set; } = null!;
}
