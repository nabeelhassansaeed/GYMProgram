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
    
    public class DailyBookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyBookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        // GET: DailyBookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailyBookings.Include(d => d.Customer).Include(d => d.booking);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        // GET: DailyBookings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyBooking = await _context.DailyBookings
                .Include(d => d.Customer)
                .Include(d => d.booking)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (dailyBooking == null)
            {
                return NotFound();
            }

            return View(dailyBooking);
        }

        [Authorize(Roles = "Administrator,User")]
        // GET: DailyBookings/Create
        public async Task<IActionResult> Create(Guid? sectionGuid, DateTime? date)
        {
            if (date != null || date != DateTime.MinValue)
            {
                string SQL = string.Format("select bo.* from Bookings bo where QTYCustomers>(select COUNT(*) from DailyBookings " +
                    "where BookingGuid=bo.Guid and Status=0) and bo.SectionGuid='{0}' ", sectionGuid);
                List<Bookings> bookings = await _context.Bookings.FromSqlRaw(SQL).ToListAsync();
                return PartialView("_GetAvailableBookings", bookings.Where(b => b.StartDate.Date == date).ToList().OrderBy(b => b.StartDate));
            }
            return Json(new { messag = "يجب ادخال التاريخ" });
        }

        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid customerguid, Guid bookingGuid)
        {
            
            if (bookingGuid != Guid.Empty || bookingGuid != null && customerguid != Guid.Empty || customerguid != null)
            {
                var booking = _context.Bookings.Find(bookingGuid);
                DailyBooking dailyBooking = new DailyBooking
                {
                    Guid = Guid.NewGuid(),
                    CUGuid = customerguid,
                    BookingGuid = booking.Guid,
                    Date = DateTime.Now,
                    Status = false,
                    Number = (_context.DailyBookings.Max(t => (int?)t.Number) ?? 0) + 1,
                    EndDate = booking.EndDate,
                    StartDate = booking.StartDate
                };
                // التحقق من وجود حجز في نفس اليوم
                var dail = _context.DailyBookings.Where(d => d.StartDate.Date == booking.StartDate.Date && d.CUGuid == customerguid&&d.Status==false).ToList();
                if (dail.Count>0)
                {
                    goto ex;
                }
                // التحقق من اكتمال الحجوزات لهذا الوقت
                string SQL = string.Format("select bo.* from Bookings bo where QTYCustomers>(select COUNT(*) from DailyBookings " +
                    "where BookingGuid=bo.Guid and Status=0) and bo.Guid='{0}' ", booking.Guid);
                List<Bookings> bookings = await _context.Bookings.FromSqlRaw(SQL).ToListAsync();
                if (bookings.Count<=0)
                {
                    goto ex;
                }
                _context.Add(dailyBooking);
                await _context.SaveChangesAsync();

                ex:
                //return PartialView("_GetCustomerBookings", bookings);
                return Json(new { messag = "تمت الاضافة بنجاح" });
            }
            return Json(new { messag = "خطأ" });
        }

        [Authorize(Roles = "Administrator,User")]
        // GET: DailyBookings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyBooking = await _context.DailyBookings.FindAsync(id);
            if (dailyBooking == null)
            {
                return NotFound();
            }
            ViewData["CUGuid"] = new SelectList(_context.Customers, "Guid", "Name", dailyBooking.CUGuid);
            ViewData["BookingGuid"] = new SelectList(_context.Bookings, "Guid", "Guid", dailyBooking.BookingGuid);
            return View(dailyBooking);
        }

        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid customerguid, Guid bookingGuid)
        {
            if (bookingGuid != Guid.Empty || bookingGuid != null && customerguid != Guid.Empty || customerguid != null)
            {
                var booking = _context.Bookings.Find(bookingGuid);
                DailyBooking dailyBooking = _context.DailyBookings.Find(bookingGuid);
                dailyBooking.Status = true;
                _context.Update(dailyBooking);
                await _context.SaveChangesAsync();
                return Json(new { messag = "تم الغاء الموعد بنجاح" });
            }
            return Json(new { messag = "خطأ" });
        }

        [Authorize(Roles = "Administrator")]
        // GET: DailyBookings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyBooking = await _context.DailyBookings
                .Include(d => d.Customer)
                .Include(d => d.booking)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (dailyBooking == null)
            {
                return NotFound();
            }

            return View(dailyBooking);
        }

        [Authorize(Roles = "Administrator")]
        // POST: DailyBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dailyBooking = await _context.DailyBookings.FindAsync(id);
            _context.DailyBookings.Remove(dailyBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        public async Task<IActionResult> GetBookings(Guid customerguid, DateTime date)
        {
            if (date != DateTime.MinValue && customerguid != null)
            {
                var bookings = await _context.DailyBookings.Where(d => d.CUGuid == customerguid && d.StartDate.Date == date.Date
                                                        && d.StartDate.Date >= DateTime.Now.Date
                                                        &&d.Status==false)
                    
                                                           .OrderBy(d => d.StartDate).ToListAsync();
                return PartialView("_GetCustomerBookings", bookings);
            }
            if (customerguid != null)
            {
                var bookings = await _context.DailyBookings.Where(d => d.CUGuid == customerguid && d.StartDate.Date >= DateTime.Now.Date && d.Status == false)
                                                           .OrderBy(d => d.StartDate).ToListAsync();
                return PartialView("_GetCustomerBookings", bookings);
            }
            return View();
        }

        [Authorize(Roles = "Administrator,User")]
        public async Task<List<DailyBooking>> SearchCustomerBookingsforguid(Guid customerguid, DateTime date)
        {
            List<DailyBooking> bookings;
            if (date != DateTime.MinValue && customerguid != null)
            {
                bookings = await _context.DailyBookings.Where(d => d.CUGuid == customerguid && d.StartDate.Date == date.Date
                                                        && d.StartDate.Date >= DateTime.Now.Date)
                                                          .OrderBy(d => d.StartDate).ToListAsync();
                return bookings;
            }
            if (customerguid != null)
            {
                bookings = await _context.DailyBookings.Where(d => d.CUGuid == customerguid && d.StartDate.Date >= DateTime.Now.Date)
                                                          .OrderBy(d => d.StartDate).ToListAsync();
                return bookings;
            }
            bookings = new List<DailyBooking>();
            return bookings;
        }
        private bool DailyBookingExists(Guid id)
        {
            return _context.DailyBookings.Any(e => e.Guid == id);
        }
    }
}
