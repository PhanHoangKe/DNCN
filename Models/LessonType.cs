using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class LessonType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
