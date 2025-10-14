using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int CourseId { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
