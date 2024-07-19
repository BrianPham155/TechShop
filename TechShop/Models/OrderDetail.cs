using System;
using System.Collections.Generic;

namespace TechShop.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OderId { get; set; }

    public int? ProductId { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public int? Total { get; set; }

    public DateTime? ShipDate { get; set; }

    public virtual Order? Oder { get; set; }

    public virtual Product? Product { get; set; }
}
