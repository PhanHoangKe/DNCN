using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class QuizAttempt
{
    [Key]
    public int AttemptId { get; set; }

    public int QuizId { get; set; }

    public int UserId { get; set; }

    public decimal? Score { get; set; }

    public int? TotalQuestions { get; set; }

    public int? CorrectAnswers { get; set; }

    public bool? IsPassed { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? TimeSpent { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual Users Users { get; set; } = null!;
}
