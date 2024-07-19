using System;
using System.Collections.Generic;

namespace TechShop.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? Provinces { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool Active { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
