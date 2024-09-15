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
    public class CourseContentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseContents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseContent.Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContent
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseContent == null)
            {
                return NotFound();
            }

            return View(courseContent);
        }

        // GET: CourseContents/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName");
            return View();
        }

        // POST: CourseContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,ContentTitle,ContentDetail,SortOrder")] CourseContent courseContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", courseContent.CourseId);
            return View(courseContent);
        }

        // GET: CourseContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContent.FindAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", courseContent.CourseId);
            return View(courseContent);
        }

        // POST: CourseContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,ContentTitle,ContentDetail,SortOrder")] CourseContent courseContent)
        {
            if (id != courseContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseContentExists(courseContent.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", courseContent.CourseId);
            return View(courseContent);
        }

        // GET: CourseContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContent
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseContent == null)
            {
                return NotFound();
            }

            return View(courseContent);
        }

        // POST: CourseContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseContent = await _context.CourseContent.FindAsync(id);
            if (courseContent != null)
            {
                _context.CourseContent.Remove(courseContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseContentExists(int id)
        {
            return _context.CourseContent.Any(e => e.Id == id);
        }
    }
}
