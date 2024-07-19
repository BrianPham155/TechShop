using Microsoft.AspNetCore.Mvc;
using TechShop.Extension;
using TechShop.ModelViews;

namespace TechShop.Controllers.Components
{
    public class NumberCartViewComponents : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
