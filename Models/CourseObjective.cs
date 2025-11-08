using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class CourseObjective
{
    public int ObjectiveId { get; set; }

    public int CourseId { get; set; }

    public string Objective { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public virtual Course Course { get; set; } = null!;
}
