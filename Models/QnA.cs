using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class QnA
{
    public int QnAid { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    public string QuestionTitle { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public bool? IsAnswered { get; set; }

    public bool? IsFeatured { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<QnAanswer> QnAanswers { get; set; } = new List<QnAanswer>();

    public virtual User User { get; set; } = null!;
}
