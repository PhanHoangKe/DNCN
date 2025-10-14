using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Users Users { get; set; } = null!;
}
