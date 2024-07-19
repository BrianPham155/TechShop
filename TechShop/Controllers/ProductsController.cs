using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Models;
using System.Linq;

namespace TechShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TechShopContext _context;
        private const int PageSize = 6; // Set the number of items per page

        public ProductsController(TechShopContext context)
        {
            _context = context;
        }
        [Route("product.html", Name = "Products")]
        public IActionResult Index(int page = 1)
        {
            
            var totalProducts = _context.Products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
            var products = _context.Products
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(products);
        }

        [Route("/{Alias}.html", Name = "ListProducts")]
        public IActionResult List(string Alias, int page = 1)
        {
            var danhmuc = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Alias == Alias);
            var totalProducts = _context.Products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
            var products = _context.Products
                .OrderBy(p => p.ProductId)
                .Where(x => x.CatId == danhmuc.CatId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentCategory = danhmuc;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(products);
        }
        [Route("/product/{Alias}-{id}.html", Name = "ProductsDetail")]
        public IActionResult Details(int id)
                {
                    var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                    if (product == null)
                    {
                        return RedirectToAction("Index");
                    }

                 var lsProduct = _context.Products.AsNoTracking()
                .Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(3)
                .ToList();
                    
                    ViewBag.SanPham = lsProduct;
                    return View(product);
                }
            }
        }

