using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class CourseRequirement
{
    [Key]
    public int RequirementId { get; set; }

    public int CourseId { get; set; }

    public string Requirement { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public virtual Course Course { get; set; } = null!;
}
