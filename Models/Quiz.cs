using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public int LessonId { get; set; }

    public string QuizTitle { get; set; } = null!;

    public string? Description { get; set; }

    public int? TimeLimit { get; set; }

    public decimal? PassingScore { get; set; }

    public int? MaxAttempts { get; set; }

    public bool? ShowCorrectAnswers { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
