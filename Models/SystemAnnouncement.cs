using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class SystemAnnouncement
{
    [Key]
    public int AnnouncementId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;
}
