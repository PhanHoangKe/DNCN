using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class LessonAttachment
{
    public int AttachmentId { get; set; }

    public int LessonId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public long? FileSize { get; set; }

    public string? FileType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;
}
