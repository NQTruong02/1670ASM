using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_AppWeb_Bicycle_Shop.Data;
using ASM_AppWeb_Bicycle_Shop.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    [Route("Admin")]
    [Authorize(Roles ="Admin")]
    public class ShopsController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;

        public ShopsController(ASM_AppWeb_Bicycle_ShopContext context)
        {
            _context = context;
        }

        // GET: Admin/StoreManager/Index
        [Route("StoreManager/Index")]
        public async Task<IActionResult> Index()
        {
            var data = await _context.Shop.ToListAsync();
            if (data != null & data.Count != 0)
            {
                ViewBag.mess = "True";
            }
            else
            {
                ViewBag.mess = "False";
            }
            return _context.Shop != null ? 
                          View(await _context.Shop.ToListAsync()) :
                          Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.Shop'  is null.");
        }

        // POST: Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ProductName,ProductPrice,Quantity,CustomerName,Purchase_date,TotalRevenue")] Shop shop)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(shop);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(shop);
        //}

        // GET: Admin/StoreManager/Edit/5
        [Route("StoreManager/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shop == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Admin/StoreManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("StoreManager/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ProductPrice,Quantity,CustomerName,Purchase_date,TotalRevenue")] Shop shop)
        {
            if (id != shop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.Id))
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
            return View(shop);
        }

        // GET: Admin/StoreManager/Delete/5
        [Route("StoreManager/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shop == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Admin/StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("StoreManager/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shop == null)
            {
                return Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.Shop'  is null.");
            }
            var shop = await _context.Shop.FindAsync(id);
            if (shop != null)
            {
                _context.Shop.Remove(shop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
          return (_context.Shop?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
