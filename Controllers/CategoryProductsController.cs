using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_AppWeb_Bicycle_Shop.Data;
using ASM_Bicycle_Shops.Models;
using ASM_AppWeb_Bicycle_Shop.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryProductsController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;

        public CategoryProductsController(ASM_AppWeb_Bicycle_ShopContext context)
        {
            _context = context;
        }

        // GET: Category/index
        [Route("Category/Index")]
        public async Task<IActionResult> Index()
        {
              return _context.CategoryProduct != null ? 
                          View(await _context.CategoryProduct.ToListAsync()) :
                          Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.CategoryProduct'  is null.");
        }

        // GET: Category/Details/5
        [Route("Category/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct
                .FirstOrDefaultAsync(m => m.CategoryProductId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // GET: Category/Create
        [Route("Category/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Category/Create")]
        public async Task<IActionResult> Create([Bind("CategoryProductId,CategoryProductName,CategoryProductDecription")] CategoryProduct categoryProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryProduct);
        }

        // GET: Category/Edit/5
        [Route("Category/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct.FindAsync(id);
            TempData["oldName"] = categoryProduct.CategoryProductName;
            if (categoryProduct == null)
            {
                return NotFound();
            }
            return View(categoryProduct);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Category/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryProductId,CategoryProductName,CategoryProductDecription")] CategoryProduct categoryProduct)
        {
            if (id != categoryProduct.CategoryProductId)
            {
                return NotFound();
            }

            //Update product
            var oldCate = TempData["oldName"].ToString();
            if (oldCate != null)
            {
                var product = _context.Product.ToList<Product>();
                foreach (var productItem in product)
                {
                    if (productItem.CategoryName == oldCate)
                    {
                        productItem.CategoryName = categoryProduct.CategoryProductName;
                        _context.Update(productItem);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Update Category New
                    _context.Update(categoryProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryProductExists(categoryProduct.CategoryProductId))
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
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Delete/5
        [Route("Category/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct
                .FirstOrDefaultAsync(m => m.CategoryProductId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Category/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryProduct == null)
            {
                return Problem("Entity set 'ASM_Bicycle_ShopsContext.CategoryProduct'  is null.");
            }
            var categoryProduct = await _context.CategoryProduct.FindAsync(id);

            if (categoryProduct != null)
            {
                _context.CategoryProduct.Remove(categoryProduct);
                //Update for product
                string cateName = categoryProduct.CategoryProductName;
                var product = _context.Product.ToList<Product>();
                foreach (var productItem in product)
                {
                    if (productItem.CategoryName == cateName)
                    {
                        productItem.CategoryName = "null";
                        _context.Update(productItem);
                        await _context.SaveChangesAsync();
                    }
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryProductExists(int id)
        {
          return (_context.CategoryProduct?.Any(e => e.CategoryProductId == id)).GetValueOrDefault();
        }
    }
}
