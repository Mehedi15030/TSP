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
    public class TrainerCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainerCourses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TrainerCourse.Include(t => t.Course).Include(t => t.Trainer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TrainerCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerCourse = await _context.TrainerCourse
                .Include(t => t.Course)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainerCourse == null)
            {
                return NotFound();
            }

            return View(trainerCourse);
        }

        // GET: TrainerCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName");
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "TrainerName");
            return View();
        }

        // POST: TrainerCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,TrainerId,IsAvailable")] TrainerCourse trainerCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainerCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", trainerCourse.CourseId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "TrainerName", trainerCourse.TrainerId);
            return View(trainerCourse);
        }

        // GET: TrainerCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerCourse = await _context.TrainerCourse.FindAsync(id);
            if (trainerCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", trainerCourse.CourseId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "TrainerName", trainerCourse.TrainerId);
            return View(trainerCourse);
        }

        // POST: TrainerCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,TrainerId,IsAvailable")] TrainerCourse trainerCourse)
        {
            if (id != trainerCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainerCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerCourseExists(trainerCourse.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseName", trainerCourse.CourseId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "TrainerName", trainerCourse.TrainerId);
            return View(trainerCourse);
        }

        // GET: TrainerCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerCourse = await _context.TrainerCourse
                .Include(t => t.Course)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainerCourse == null)
            {
                return NotFound();
            }

            return View(trainerCourse);
        }

        // POST: TrainerCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainerCourse = await _context.TrainerCourse.FindAsync(id);
            if (trainerCourse != null)
            {
                _context.TrainerCourse.Remove(trainerCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerCourseExists(int id)
        {
            return _context.TrainerCourse.Any(e => e.Id == id);
        }
    }
}
