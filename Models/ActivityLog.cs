using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class ActivityLog
{
    [Key]
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public string? Details { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Users? Users { get; set; }
}
