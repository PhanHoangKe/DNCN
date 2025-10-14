using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class StudentAnswer
{
    [Key]
    public int StudentAnswerId { get; set; }

    public int AttemptId { get; set; }

    public int QuestionId { get; set; }

    public int AnswerId { get; set; }

    public bool? IsCorrect { get; set; }

    public DateTime? AnsweredAt { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual QuizAttempt Attempt { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
