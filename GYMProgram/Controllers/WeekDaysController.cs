using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYMProgram.Data;
using GYMProgram.Models;
using GYMProgram.BusinessFunctional;
using GYMProgram.ViewModels;

namespace GYMProgram.Controllers
{
    public class WeekDaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DayHours _dayHours;
        private readonly ListWeekDays _listWeekDays; 
        public WeekDaysController(ApplicationDbContext context)
        {
            _context = context;
            _dayHours = new DayHours();
            _listWeekDays = new ListWeekDays();
        }
       
        // GET: WeekDays
        public async Task<IActionResult> Index(Guid id)
        {
            var applicationDbContext = _context.WeekDays.Where(w=>w.HeadGuid==id).Include(w => w.WeekDaysHead).OrderBy(w=>w.DayNumber).ThenBy(w=>w.WorkStartHour);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WeekDays/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays
                .Include(w => w.WeekDaysHead)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (weekDay == null)
            {
                return NotFound();
            }

            return View(weekDay);
        }

        // GET: WeekDays/Create
        public IActionResult Create()
        {
            WeekDay weekDay = new WeekDay();
            
            weekDay.Number = (_context.WeekDays.Max(t => (int?)t.Number) ?? 0) + 1;
            ViewData["WeekDaysHeads"] = new SelectList(_context.WeekDaysHeads, "Guid", "Notes");
            var listDays = _listWeekDays.GetListWeekDays();
            ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
            //ViewData["Hours"] = new SelectList(_dayHours.GetHoursList(), "HourValue", "HourText");
            return View(weekDay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( WeekDay weekDay,Guid headGuid)
        {
            if (ModelState.IsValid)
            {
                if (checkDuplication(weekDay.DayNumber, DateTime.Parse(weekDay.WorkStartHour.ToString("hh:mm tt"))))
                {
                    ModelState.AddModelError("", "الوقت واليوم المدخل مكرر لا يمكن اضافته");
                    goto ex;
                }
                TimeSpan tim =  weekDay.WorkEndHour- weekDay.WorkStartHour;
                weekDay.HeadGuid = headGuid;
               var listDay = _listWeekDays.FindWeekDays(weekDay.DayNumber);
                weekDay.DayName = listDay.DayName;
                weekDay.DayLatinName = listDay.DayLatinName;
                weekDay.Minutes = tim.ToString();
                weekDay.Guid = Guid.NewGuid();
                weekDay.Number= (_context.WeekDays.Max(t => (int?)t.Number) ?? 0) + 1; ;
                _context.Add(weekDay);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index),new { id= headGuid });
            }
        ex:
            var listDays = _listWeekDays.GetListWeekDays();
            ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
            ViewData["WeekDaysHeads"] = new SelectList(_context.WeekDaysHeads, "Guid", "Notes", weekDay.WeekDaysHead);
            //ViewData["Hours"] = new SelectList(_dayHours.GetHoursList(), "HourId", "HourName");
            return View(weekDay);
        }

        // GET: WeekDays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays.FindAsync(id);
            if (weekDay == null)
            {
                return NotFound();
            }
            var listDays = _listWeekDays.GetListWeekDays();
            ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
            ViewData["WeekDaysHeads"] = new SelectList(_context.WeekDaysHeads, "Guid", "Notes", weekDay.WeekDaysHead);
            //ViewData["Hours"] = _dayHours.GetHoursList();
            return View(weekDay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,WeekDay weekDay,Guid headGuid)
        {
            if (id != weekDay.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                weekDay.HeadGuid = headGuid;
                var listDay = _listWeekDays.FindWeekDays(weekDay.DayNumber);
                weekDay.DayName = listDay.DayName;
                weekDay.DayLatinName = listDay.DayLatinName;
                TimeSpan tim = weekDay.WorkEndHour - weekDay.WorkStartHour;
                weekDay.Minutes = tim.ToString();
                try
                {
                    _context.Update(weekDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekDayExists(weekDay.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { id=headGuid});
            }
            var listDays = _listWeekDays.GetListWeekDays();
            ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
            ViewData["WeekDaysHead"] = new SelectList(_context.Sections, "Guid", "Name", weekDay.WeekDaysHead);
            return View(weekDay);
        }

        [HttpGet]
        public async Task<IActionResult> CopyDay()
        {
            var listDays =  _listWeekDays.GetListWeekDays();
            ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
            ViewData["WeekDaysHeads"] = new SelectList(_context.WeekDaysHeads, "Guid", "Notes");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CopyDay(Guid headGuid, int FromDayNumber,int ToDayNumber)
        {
            if (headGuid == Guid.Empty ||FromDayNumber==null||ToDayNumber==null)
            {
                return NotFound();
            }
            var weekDays = _context.WeekDays.Where(s => s.DayNumber == FromDayNumber).ToList().OrderBy(s=>s.Number);
            WeekDays listDay = new WeekDays();
            WeekDay day;
           
            foreach (var item in weekDays)
            {               
                listDay = _listWeekDays.FindWeekDays(ToDayNumber);
                day = item;

                day.DayNumber = listDay.DayNumber;
                day.DayName = listDay.DayName;
                day.DayLatinName = listDay.DayLatinName;
                day.Number = (_context.WeekDays.Max(t => (int?)t.Number) ?? 0) + 1;
                day.Minutes= (item.WorkEndHour - item.WorkStartHour).ToString();
                day.Guid = Guid.NewGuid();

                if (checkDuplication(day.DayNumber, DateTime.Parse(day.WorkStartHour.ToString("hh:mm tt"))))
                {
                    
                    ModelState.AddModelError("","الوقت واليوم المدخل مكرر لا يمكن اضافته");
                    var listDays = _listWeekDays.GetListWeekDays();
                    ViewData["ListWeekDays"] = new SelectList(listDays, "DayNumber", "DayName");
                    ViewData["WeekDaysHeads"] = new SelectList(_context.WeekDaysHeads, "Guid", "Notes");

                    return View(day);
                }

                _context.Add(day);
                await _context.SaveChangesAsync();
                //return View("Index");
            }
            return RedirectToAction(nameof(Index),new { id=headGuid});
        }

        public bool checkeDuplicationBooking(Guid SectionGuid,DateTime StartDate)
        {
            var booking = _context.Bookings.Where(b => b.SectionGuid == SectionGuid && b.StartDate == StartDate).ToList();
            if (booking.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkDuplication (int DayNumber,DateTime time)
        {
            var weekDays = _context.WeekDays.Where(w => w.DayNumber == DayNumber && w.WorkStartHour == DateTime.Parse(time.ToShortTimeString())).ToList();
            if (weekDays.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: WeekDays/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays
                .Include(w => w.WeekDaysHead)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (weekDay == null)
            {
                return NotFound();
            }

            return View(weekDay);
        }

        // POST: WeekDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var weekDay = await _context.WeekDays.FindAsync(id);
            _context.WeekDays.Remove(weekDay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeekDayExists(Guid id)
        {
            return _context.WeekDays.Any(e => e.Guid == id);
        }
    }
}
