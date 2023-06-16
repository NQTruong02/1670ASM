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
    [Authorize(Roles ="Admin")]
    [Route("Admin")]
    public class EventsController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;

        public EventsController(ASM_AppWeb_Bicycle_ShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Events/Index
        [Route("Events/Index")]
        public async Task<IActionResult> Index()
        {
              return _context.CalendarEvent != null ? 
                          View(await _context.CalendarEvent.ToListAsync()) :
                          Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.CalendarEvent'  is null.");
        }


        // GET: Admin/Events/Index
        [Route("Events/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // GET: Admin/Events/Index
        [Route("Events/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End,Text,Color")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendarEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calendarEvent);
        }

        // GET: Admin/Events/Edit/5
        [Route("Events/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CalendarEvent == null)
            {
                return NotFound();
            }

            var calendarEvent = await _context.CalendarEvent.FindAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return View(calendarEvent);
        }

        // POST: Admin/Events/Edit/5
        [Route("Events/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Text,Color")] CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarEventExists(calendarEvent.Id))
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
            return View(calendarEvent);
        }

        // GET: Admin/Events/Delete/Edit/5
        [Route("Events/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CalendarEvent == null)
            {
                return NotFound();
            }

            var calendarEvent = await _context.CalendarEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarEvent == null)
            {
                return NotFound();
            }

            return View(calendarEvent);
        }

        // Post: Admin/Events/Delete/Edit/5
        [Route("Events/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CalendarEvent == null)
            {
                return Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.CalendarEvent'  is null.");
            }
            var calendarEvent = await _context.CalendarEvent.FindAsync(id);
            if (calendarEvent != null)
            {
                _context.CalendarEvent.Remove(calendarEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarEventExists(int id)
        {
          return (_context.CalendarEvent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
