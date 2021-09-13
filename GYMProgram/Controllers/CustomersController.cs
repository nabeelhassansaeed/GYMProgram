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
    
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Customers.Include(c => c.GYM);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.GYM)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer, string GYM_Guid)
        {
            customer.GYMGuid = Guid.Parse(GYM_Guid);
            if (ModelState.IsValid)
            {
                customer.Guid = Guid.NewGuid();
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", customer.GYMGuid);
            return View(customer);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", customer.GYMGuid);
            return View(customer);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Customer customer, string GYM_Guid)
        {
            if (id != customer.Guid)
            {
                return NotFound();
            }
            customer.GYMGuid = Guid.Parse(GYM_Guid);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Guid))
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
            ViewData["GYMGuid"] = new SelectList(_context.GYMs, "Guid", "Name", customer.GYMGuid);
            return View(customer);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.GYM)
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Administrator,User")]
        [HttpGet]
        public async Task<IActionResult> CheckSubscription()
        {
            //ViewData["Customers"] = new SelectList(_context.Customers, "Guid", "Name");
            return View();
        }
        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        public async Task<IActionResult> CheckSubscription(string CustomerID)
        {

            //var customer1 = from s in _context.Customers
            //               where (s.IDNumber + s.Mobile + s.Name + s.LatinName).Contains(CustomerID)
            //               //EF.Functions.Like(s.IDNumber+s.Mobile+s.Name+s.LatinName, "%"+ CustomerID + "%")
            //               select s;
            string SQL = string.Format("select * from Customers where Name like N'%{0}%' or IDNumber like N'%{0}%' or Mobile like N'%{0}%' " +
                "or LatinName like N'%{0}%' or Guid like N'%{0}%'", CustomerID);
            List<Customer> customer = await _context.Customers.FromSqlRaw(SQL).ToListAsync();
          
            if (customer.ToList().Count == 1)
            {
                ViewData["SectionGuid"] = new SelectList(_context.Sections.Where(c => c.GYMGuid == customer.LastOrDefault().GYMGuid), "Guid", "Name");
                return View(customer);
            }
            else
            {
                if (customer.ToList().Count > 1)
                {
                    ViewData["SectionGuid"] = new SelectList(_context.Sections.Where(c => c.GYMGuid == customer.LastOrDefault().GYMGuid), "Guid", "Name");
                    return View(customer);
                }
                else
                {
                    ModelState.AddModelError("", "لم يتم العثور على المشترك");
                    return View();
                }
            }
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.Guid == id);
        }
    }
}
