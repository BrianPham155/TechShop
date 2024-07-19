using System;
using System.Collections.Generic;

namespace TechShop.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public bool Published { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
