using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYMProgram.Data;
using GYMProgram.Models;

namespace GYMProgram.Controllers
{
    public class UnavailableDatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnavailableDatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UnavailableDates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UnavailableDates.Include(u => u.Section);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UnavailableDates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unavailableDate = await _context.UnavailableDates
                .Include(u => u.Section)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (unavailableDate == null)
            {
                return NotFound();
            }

            return View(unavailableDate);
        }

        // GET: UnavailableDates/Create
        public IActionResult Create()
        {
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name");
            return View();
        }

        // POST: UnavailableDates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Date,Notes,SectionGuid")] UnavailableDate unavailableDate)
        {
            if (ModelState.IsValid)
            {
                unavailableDate.Guid = Guid.NewGuid();
                _context.Add(unavailableDate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", unavailableDate.SectionGuid);
            return View(unavailableDate);
        }

        // GET: UnavailableDates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unavailableDate = await _context.UnavailableDates.FindAsync(id);
            if (unavailableDate == null)
            {
                return NotFound();
            }
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", unavailableDate.SectionGuid);
            return View(unavailableDate);
        }

        // POST: UnavailableDates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Guid,Date,Notes,SectionGuid")] UnavailableDate unavailableDate)
        {
            if (id != unavailableDate.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unavailableDate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnavailableDateExists(unavailableDate.Guid))
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
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", unavailableDate.SectionGuid);
            return View(unavailableDate);
        }

        // GET: UnavailableDates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unavailableDate = await _context.UnavailableDates
                .Include(u => u.Section)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (unavailableDate == null)
            {
                return NotFound();
            }

            return View(unavailableDate);
        }

        // POST: UnavailableDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var unavailableDate = await _context.UnavailableDates.FindAsync(id);
            _context.UnavailableDates.Remove(unavailableDate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnavailableDateExists(Guid id)
        {
            return _context.UnavailableDates.Any(e => e.Guid == id);
        }
    }
}
