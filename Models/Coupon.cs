using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class Coupon
{
    [Key]
    public int CouponId { get; set; }

    public string CouponCode { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public decimal? MinOrderAmount { get; set; }

    public decimal? MaxDiscount { get; set; }

    public int? UsageLimit { get; set; }

    public int? UsedCount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
