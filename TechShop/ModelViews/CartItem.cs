using TechShop.Models;

namespace TechShop.ModelViews
{
    public class CartItem
    {
        public Product sanpham { get; set; }
        public int Quantity { get; set; }

        public int TotalMoney => Quantity * sanpham.Price.Value;
    }
}
