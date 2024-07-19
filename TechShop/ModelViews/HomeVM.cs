using TechShop.Models;

namespace TechShop.ModelViews
{
    public class HomeVM
    {
        public List<TinTuc> TinTucs { get; set; }
        public List<ProductHomeVM> Products { get; set; }
    }
}
