using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_AppWeb_Bicycle_Shop.Data;
using ASM_AppWeb_Bicycle_Shop.Models;
using Microsoft.AspNetCore.Hosting;
using ASM_Bicycle_Shops.Models;
using ASM_AppWeb_Bicycle_Shop.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    public class NewsController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public NewsController(ASM_AppWeb_Bicycle_ShopContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: ManagerNews/Index
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Index")]
        public async Task<IActionResult> Index()
        {
            return _context.News != null ?
                        View(await _context.News.ToListAsync()) :
                        Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.News'  is null.");
        }

        // GET: News/Index
        [Route("News/Index")]
        public async Task<IActionResult> NewsUser()
        {
            return _context.News != null ?
                        View(await _context.News.ToListAsync()) :
                        Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.News'  is null.");
        }

        // GET: News/Details/5
        [Route("News/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: ManagerNews/Create
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Create")]
        public async Task<IActionResult> Create([Bind("NewsId,NewsTitle,NewsAuthor,NewsContent,NewsDate,EmpPhotoPath,ImageFile")] News news)
        {
            string uniqueFileName = null;
            if (news.ImageFile != null)
            {
                string ImageUpLoadFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadImg");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + news.ImageFile.FileName;

                string filepath = Path.Combine(ImageUpLoadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    news.ImageFile.CopyTo(fileStream);
                }
                news.EmpPhotoPath = "~/wwwroot/UploadImg";
                news.EmpFileName = uniqueFileName;




                if (ModelState.IsValid)
                {
                    _context.Add(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(news);
        }

        // GET: ManagerNews/Edit/5
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: ManagerNews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,NewsTitle,NewsAuthor,NewsContent,NewsDate,EmpPhotoPath,ImageFile")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            string uniqueFileName = null;
            if (news.ImageFile != null)
            {
                string ImageUpLoadFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadImg");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + news.ImageFile.FileName;

                string filepath = Path.Combine(ImageUpLoadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    news.ImageFile.CopyTo(fileStream);
                }
                news.EmpPhotoPath = "~/wwwroot/UploadImg";
                news.EmpFileName = uniqueFileName;



                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(news);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NewsExists(news.NewsId))
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
            }
            return View(news);
        }

        // GET: ManagerNews/Delete/5
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: ManagerNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        [Route("ManagerNews/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
