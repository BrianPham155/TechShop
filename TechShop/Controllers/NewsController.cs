using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class NewsController : Controller
    {
        private readonly TechShopContext _context;
        private const int PageSize = 3; // Number of items per page

        public NewsController(TechShopContext context)
        {
            _context = context;
        }

        [Route("tintuc.html", Name = "News")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var Tintuc = await _context.TinTucs
                .OrderBy(p => p.PostId) 
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)_context.TinTucs.Count() / PageSize);

            return View(Tintuc);
        }

        [Route("/tintuc/{Alias}-{id}.html", Name = "NewsDetail")]
        public IActionResult Details(int id)
        {
            var tintuc = _context.TinTucs.AsNoTracking().SingleOrDefault(x=>x.PostId == id);
            if (tintuc == null)
            {
                return RedirectToAction("Index");
            }
            return View(tintuc);
        }
    }
}