using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string OrderCode { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; } = null!;
}
