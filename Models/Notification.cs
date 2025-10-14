using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class Notification
{
    [Key]
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? Type { get; set; }

    public bool? IsRead { get; set; }

    public string? Url { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Users Users { get; set; } = null!;
}
