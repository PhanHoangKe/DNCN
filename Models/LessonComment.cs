using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class LessonComment
{
    [Key]
    public int CommentId { get; set; }

    public int LessonId { get; set; }

    public int UserId { get; set; }

    public int? ParentCommentId { get; set; }

    public string CommentText { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<LessonComment> InverseParentComment { get; set; } = new List<LessonComment>();

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual LessonComment? ParentComment { get; set; }

    public virtual Users Users { get; set; } = null!;
}
