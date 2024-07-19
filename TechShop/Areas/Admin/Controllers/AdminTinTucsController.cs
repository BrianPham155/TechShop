using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechShop.Helper;
using TechShop.Models;

namespace TechShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTinTucsController : Controller
    {
        private readonly TechShopContext _context;

        public AdminTinTucsController(TechShopContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminTinTucs
        public async Task<IActionResult> Index()
        {

            var techShopContext = _context.TinTucs;
            return View(await techShopContext.ToListAsync());
        }

        // GET: Admin/AdminTinTucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTinTucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Contents,Thumb,Published,Alias,CreatedDate,Author,Tags,IsHot,IsNewfeed,Views")] TinTuc tinTuc, IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                tinTuc.Title = Utilities.ToTitleCase(tinTuc.Title);
                if (fThumb != null && fThumb.Length > 0)
                {
                    var fileName = Path.GetFileName(fThumb.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fThumb.CopyToAsync(stream);
                    }

                    // Update the product's Thumb property with the path or name of the uploaded file
                    tinTuc.Thumb = fileName;
                }
                tinTuc.Alias = Utilities.SEOUrl(tinTuc.Title);
                tinTuc.CreatedDate = DateTime.Now;
                _context.Add(tinTuc);
                await _context.SaveChangesAsync();
                TempData["message"] = "Thêm thành công";
                return RedirectToAction(nameof(Index));
            }
            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }
            return View(tinTuc);
        }

        // POST: Admin/AdminTinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Contents,Thumb,Published,Alias,CreatedDate,Author,Tags,IsHot,IsNewfeed,Views")] TinTuc tinTuc, IFormFile fThumb)
        {
            if (id != tinTuc.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tinTuc.Title = Utilities.ToTitleCase(tinTuc.Title);
                    if (fThumb != null && fThumb.Length > 0)
                    {
                        var fileName = Path.GetFileName(fThumb.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await fThumb.CopyToAsync(stream);
                        }

                        // Update the product's Thumb property with the path or name of the uploaded file
                        tinTuc.Thumb = fileName;
                    }
                    tinTuc.Alias = Utilities.SEOUrl(tinTuc.Title);
                    _context.Update(tinTuc);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Chỉnh sửa thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinTucExists(tinTuc.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // POST: Admin/AdminTinTucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc != null)
            {
                _context.TinTucs.Remove(tinTuc);
            }

            await _context.SaveChangesAsync();
            TempData["message"] = "Xóa thành công";
            return RedirectToAction(nameof(Index));
        }

        private bool TinTucExists(int id)
        {
            return _context.TinTucs.Any(e => e.PostId == id);
        }
    }
}
