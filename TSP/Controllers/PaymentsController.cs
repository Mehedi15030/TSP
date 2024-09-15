using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSP.Data;
using TSP.Models;

namespace TSP.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payment.Include(p => p.Batch).Include(p => p.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ShowBatch()
        {
            var data = _context.Batch.Include(p => p.Course).Include(p => p.CourseCoordinator);
            return View(await data.ToListAsync());
        }


        public async Task<IActionResult> ShowStudents(int id)
        {
            var data = _context.StudentBatch.Include(p => p.Student).Include(p => p.Batch).Where(x => x.BatchId == id);
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> ShowStudentPayments(int id, int bid)
        {
            double total_fee = 0;
            var c = _context.Batch.Include(x => x.Course).FirstOrDefault(x => x.Id == bid);
            if (c != null)
            {
                total_fee = c.Course.CourseFee;
            }
            ViewBag.coursefee = total_fee;

            var data = _context.Payment.Include(p => p.Student).Include(p => p.Batch).Where(x => x.StudentId == id && x.BatchId == bid);
            return View(await data.ToListAsync());
        }


        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Batch)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_context.Batch, "Id", "BatchName");
            ViewData["StudentId"] = new SelectList(_context.Set<Student>(), "Id", "StudentName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,StudentId,PaymentDate,RecieptNo,IsFullPayment,Amount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "Id", "BatchName", payment.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Set<Student>(), "Id", "StudentName", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "Id", "BatchName", payment.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Set<Student>(), "Id", "StudentName", payment.StudentId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,StudentId,PaymentDate,RecieptNo,IsFullPayment,Amount")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
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
            ViewData["BatchId"] = new SelectList(_context.Batch, "Id", "BatchName", payment.BatchId);
            ViewData["StudentId"] = new SelectList(_context.Set<Student>(), "Id", "StudentName", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Batch)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            if (payment != null)
            {
                _context.Payment.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
    }
}
