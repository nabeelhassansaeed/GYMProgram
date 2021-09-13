using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYMProgram.Data;
using GYMProgram.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace GYMProgram.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sections
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sections.Include(s => s.GYM);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sections/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.Sections
                .Include(s => s.GYM)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // GET: Sections/Create
        public IActionResult Create()
        {
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name");
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Section section, string GId)
        {

            section.GYMGuid = Guid.Parse(GId);           
            if (ModelState.IsValid)
            {
                section.Guid = Guid.NewGuid();
                _context.Add(section);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", section.GYMGuid);
           
            var errors = ViewData.ModelState.Values.SelectMany(x => x.Errors);
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error.ErrorMessage.ToString());
            }
            return View(section);
        }

        // GET: Sections/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", section.GYMGuid);
            return View(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,Section section, string GId)
        {
            if (id != section.Guid)
            {
                return NotFound();
            }
            section.GYMGuid = Guid.Parse(GId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(section);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.Guid))
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
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", section.GYMGuid);
            return View(section);
        }

        // GET: Sections/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.Sections
                .Include(s => s.GYM)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var section = await _context.Sections.FindAsync(id);
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionExists(Guid id)
        {
            return _context.Sections.Any(e => e.Guid == id);
        }
    }
}
