using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class QnAanswer
{
    public int AnswerId { get; set; }

    public int QnAid { get; set; }

    public int UserId { get; set; }

    public string AnswerText { get; set; } = null!;

    public bool? IsAccepted { get; set; }

    public int? Votes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual QnA QnA { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
