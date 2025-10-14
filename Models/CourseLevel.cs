using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class CourseLevel
{
    public int LevelId { get; set; }

    public string LevelName { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
