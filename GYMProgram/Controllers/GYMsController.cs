using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYMProgram.Data;
using GYMProgram.Models;
using Microsoft.AspNetCore.Authorization;

namespace GYMProgram.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GYMsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GYMsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GYMs
        public async Task<IActionResult> Index()
        {
            return View(await _context.GYMs.ToListAsync());
        }

        // GET: GYMs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gYM = await _context.GYMs
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (gYM == null)
            {
                return NotFound();
            }

            return View(gYM);
        }

        // GET: GYMs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GYMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Name,LatinName,Location,Notes,Logo,Mobile")] GYM gYM)
        {
            if (ModelState.IsValid)
            {
                gYM.Guid = Guid.NewGuid();
                _context.Add(gYM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gYM);
        }

        // GET: GYMs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gYM = await _context.GYMs.FindAsync(id);
            if (gYM == null)
            {
                return NotFound();
            }
            return View(gYM);
        }

        // POST: GYMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Guid,Name,LatinName,Location,Notes,Logo,Mobile")] GYM gYM)
        {
            if (id != gYM.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gYM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GYMExists(gYM.Guid))
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
            return View(gYM);
        }

        // GET: GYMs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gYM = await _context.GYMs
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (gYM == null)
            {
                return NotFound();
            }

            return View(gYM);
        }

        // POST: GYMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var gYM = await _context.GYMs.FindAsync(id);
            _context.GYMs.Remove(gYM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GYMExists(Guid id)
        {
            return _context.GYMs.Any(e => e.Guid == id);
        }
    }
}
