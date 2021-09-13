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
    public class WeekDaysHeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeekDaysHeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WeekDaysHeads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WeekDaysHeads.Include(w => w.weekDays).Include(w => w.Section);
            return View(await applicationDbContext.OrderBy(w=>w.FromDate).ToListAsync());
        }

        // GET: WeekDaysHeads/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDaysHead = await _context.WeekDaysHeads
                .Include(w => w.Section)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (weekDaysHead == null)
            {
                return NotFound();
            }

            return View(weekDaysHead);
        }

        // GET: WeekDaysHeads/Create
        public IActionResult Create()
        {
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( WeekDaysHead weekDaysHead)
        {
            if (ModelState.IsValid)
            {
                weekDaysHead.Guid = Guid.NewGuid();
                _context.Add(weekDaysHead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", weekDaysHead.SectionGuid);
            return View(weekDaysHead);
        }

        // GET: WeekDaysHeads/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDaysHead = await _context.WeekDaysHeads.FindAsync(id);
            if (weekDaysHead == null)
            {
                return NotFound();
            }
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", weekDaysHead.SectionGuid);
            return View(weekDaysHead);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,WeekDaysHead weekDaysHead)
        {
            if (id != weekDaysHead.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weekDaysHead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekDaysHeadExists(weekDaysHead.Guid))
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
            ViewData["SectionGuid"] = new SelectList(_context.Sections, "Guid", "Name", weekDaysHead.SectionGuid);
            return View(weekDaysHead);
        }

        // GET: WeekDaysHeads/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDaysHead = await _context.WeekDaysHeads
                .Include(w => w.Section)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (weekDaysHead == null)
            {
                return NotFound();
            }

            return View(weekDaysHead);
        }


        [HttpGet]
        public async Task<IActionResult> GenerationBookings(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDaysHeads = await _context.WeekDaysHeads.FindAsync(id);
            if (weekDaysHeads == null)
            {
                return NotFound();
            }

            return View(weekDaysHeads);
        }

        [HttpPost]
        public async Task<IActionResult> GenerationBookings(Guid headGuid)
        {
            if (headGuid == null)
            {
                return NotFound();
            }
            var weekDaysHeads = await _context.WeekDaysHeads.FindAsync(headGuid);
            if (weekDaysHeads.ToDate.AddDays(1) < DateTime.Now)
            {
                ModelState.AddModelError("", "التاريخ قبل تاريخ اليوم لا يمكن توليد الحجوزات");
                return RedirectToAction(nameof(Index), "WeekDaysHeads");
            }
            else
            {
                int countdays = (weekDaysHeads.ToDate.Day + 1) - weekDaysHeads.FromDate.Day;
                Bookings booking;
                DateTime fromdate = weekDaysHeads.FromDate; ;         
                List< WeekDay> weekDay;
                for (int i = 0; i < countdays; i++)
                {                  
                    var dayOfWeek = int.Parse(fromdate.DayOfWeek.ToString("d"));
                    weekDay = _context.WeekDays.Where(w => w.DayNumber == dayOfWeek).ToList();
                    foreach (var item in weekDay.OrderBy(o => o.Number))
                    {
                        booking = new Bookings
                        {
                            Guid = Guid.NewGuid(),
                            Number = (_context.Bookings.Max(t => (int?)t.Number) ?? 0) + 1,
                            SectionGuid = weekDaysHeads.SectionGuid,
                            Status = 0,
                            Date = DateTime.Now,
                            StartDate =DateTime.Parse(fromdate.ToString("yyyy-MM-dd") +" " + item.WorkStartHour.ToString("hh:mm tt")),
                            EndDate = DateTime.Parse(fromdate.ToString("yyyy-MM-dd") + " " + item.WorkEndHour.ToString("hh:mm tt")),
                            QTYCustomers = item.QTYCustomers
                        };
                        // التحقق من تكرار خطة الحجز
                        if (checkeDuplicationBooking(booking.SectionGuid.Value,booking.StartDate))
                        {
                            goto ex;
                        }
                        // التحقق من هل هذا التاريخ متاح او غير متاح
                        if (!checkeAvailableBooking(booking.SectionGuid.Value, booking.StartDate))
                        {
                            goto ex;
                        }
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                    ex: 
                        fromdate = fromdate;
                    }
                    fromdate = fromdate.AddDays(1);
                }
                return RedirectToAction(nameof(Index), "Bookings");
            }
        }


        // POST: WeekDaysHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var weekDaysHead = await _context.WeekDaysHeads.FindAsync(id);
            _context.WeekDaysHeads.Remove(weekDaysHead);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool checkeDuplicationBooking(Guid SectionGuid, DateTime StartDate)
        {
            var booking = _context.Bookings.Where(b => b.SectionGuid == SectionGuid && b.StartDate == StartDate).ToList();
            if (booking.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkeAvailableBooking(Guid SectionGuid, DateTime StartDate)
        {
            var booking = _context.UnavailableDates.Where(b => b.SectionGuid == SectionGuid && b.Date.Date == StartDate.Date).ToList();
            if (booking.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool WeekDaysHeadExists(Guid id)
        {
            return _context.WeekDaysHeads.Any(e => e.Guid == id);
        }
    }
}
