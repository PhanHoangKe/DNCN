using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class Answer
{
    [Key]
    public int AnswerId { get; set; }

    public int QuestionId { get; set; }

    public string AnswerText { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public int? DisplayOrder { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
