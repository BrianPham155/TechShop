using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TechShop.Extension;
using TechShop.Models;
using TechShop.ModelViews;

namespace TechShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly TechShopContext _context;


        public ShoppingCartController(TechShopContext context)
        {
            _context = context;
        }

        // Lấy hoặc thiết lập giỏ hàng từ Session
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == null)
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
            set
            {
                HttpContext.Session.Set<List<CartItem>>("GioHang", value);
            }
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? Quantity)
        {
            try
            {
                List<CartItem> giohang = GioHang;
                CartItem item = giohang.SingleOrDefault(x => x.sanpham.ProductId == productID);
                if (item != null)
                {
                    item.Quantity += Quantity ?? 1;
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    if (hh == null)
                    {
                        return Json(new { success = false, message = "Product not found" });
                    }
                    item = new CartItem
                    {
                        Quantity = Quantity ?? 1,
                        sanpham = hh
                    };
                    giohang.Add(item);
                }
                GioHang = giohang; // Lưu lại giỏ hàng vào Session
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult RemoveFromCart(int productID)
        {
            try
            {
                List<CartItem> giohang = GioHang;
                CartItem item = giohang.SingleOrDefault(x => x.sanpham.ProductId == productID);
                if (item != null)
                {
                    giohang.Remove(item);
                    GioHang = giohang; // Lưu lại giỏ hàng vào Session
                }
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int quantity)
        {
            try
            {
                List<CartItem> giohang = GioHang;
                CartItem item = giohang.SingleOrDefault(x => x.sanpham.ProductId == productID);
                if (item != null)
                {
                    if (quantity <= 0)
                    {
                        giohang.Remove(item);
                    }
                    else
                    {
                        item.Quantity = quantity;
                    }
                    GioHang = giohang; // Lưu lại giỏ hàng vào Session
                }
                int totalMoney = giohang.Sum(x => x.TotalMoney);
                return Json(new { success = true, totalMoney = totalMoney, itemTotal = item != null ? item.Quantity * item.sanpham.Price.Value : 0 });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        // Hiển thị giỏ hàng
        [Route("cart.html", Name = "cart")]
        public IActionResult Index()
        {
            var giohang = GioHang;
            decimal totalMoney = giohang.Sum(x => x.TotalMoney);
            ViewBag.TotalMoney = totalMoney;
            return View(giohang);
        }

        // Trang Checkout (GET)
        [HttpGet]
        [Route("cart/checkout", Name = "Checkout")]
        public IActionResult Checkout()
        {
            var giohang = GioHang;
            decimal totalMoney = giohang.Sum(x => x.TotalMoney);
            ViewBag.CartItems = giohang;
            ViewBag.TotalMoney = totalMoney;
            return View(new CheckoutModel());
        }

        // Checkout (POST)
        [HttpPost]
        [Route("cart/checkout")]
        public IActionResult Checkout(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                List<CartItem> giohang = GioHang;
                if (giohang.Count > 0)
                {
                    // Lưu đơn hàng vào cơ sở dữ liệu
                    var customer = _context.Customers.FirstOrDefault(c => c.Email == model.Email);

                    if (customer != null)
                    {
                        customer.Address = model.Address;
                        customer.Ward = model.Ward;
                        customer.District = model.District; 
                        customer.Provinces = model.Province;    

                        Order order = new Order
                        {
                            CustomerId = customer.CustomerId,
                            OrderDate = DateTime.Now,
                            TotalMoney = giohang.Sum(x => x.TotalMoney),
                            // Các thông tin khác của đơn hàng (tùy vào yêu cầu của bạn)
                        };
                        _context.Customers.Update(customer);
                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
                        return View(model);
                    }

                    // Xóa giỏ hàng sau khi thanh toán thành công
                    HttpContext.Session.Set<List<CartItem>>("GioHang", new List<CartItem>());

                    // Chuyển hướng đến trang thanh toán thành công
                    return RedirectToAction("CheckoutSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                }
            }
            return View(model);
        }

        // Thông báo thanh toán thành công
        [Route("cart/checkout-success", Name = "CheckoutSuccess")]
        public IActionResult CheckoutSuccess()
        {
            return View();
        }
    }
}
