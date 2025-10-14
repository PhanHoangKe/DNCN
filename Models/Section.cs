using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public string SectionTitle { get; set; } = null!;

    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
