using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public StudentCourseController(DataContext context, IUserHelper userHelper)
        {
            _context= context;
            _userHelper= userHelper;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (User == null)
            {
                return NotFound();
            }

            User user1 = await _context.Users
            .Include(u => u.StudentCourses)
            .ThenInclude(s => s.Course)
            .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (user1 == null)
            {
                return NotFound();
            }

            return View(user1);
        }
        public async Task<IActionResult> IndexCourse()
        {
            return View(await _context.Courses
                .Include(c => c.StudentCourses)
                .ToListAsync());
        }

        public async Task<IActionResult> DetailsCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses
                .Include(c => c.StudentCourses)
                .ThenInclude(s => s.User)
                .Include(c => c.StudentCourses)
                .ThenInclude(s => s.Grades)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public async Task<IActionResult> DetailsGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentCourse studentCourse = await _context.StudentCourses
                .Include(s => s.Course)
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }



        public async Task<IActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}


            StudentCourse studentCourse = await _context.StudentCourses
             .Include(s => s.Course)
             .Include(s=>s.Grades)
            .FirstOrDefaultAsync(u => u.Id == id);


            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }


        public async Task<IActionResult> AddGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentCourse studentCourse = await _context.StudentCourses.FindAsync(id);

            if (studentCourse == null)
            {
                return NotFound();
            }

            GradeViewModel model = new()
            {
                StudentCourseId = studentCourse.Id,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(GradeViewModel model)
        {
            // if (ModelState.IsValid)
            //{
            try
            {
                Grade grade = new()
                {


                    StudentCourse = await _context.StudentCourses.FindAsync(model.StudentCourseId),
                    Grades = model.Grades,
                    Percentage = model.Percentage,
                    
                };
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //TODO: remmember that duplicate is with first name and lastname

            //catch (DbUpdateException dbUpdateException)
            //{
            //    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
            //    {
            //        ModelState.AddModelError(string.Empty, "Ya existe una acudiente  con el mismo nombre para este usuario.");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            //    }
            //}
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(model);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentCourse studentCourse = await _context.StudentCourses
                .Include(c => c.User)
                .Include(c => c.Grades)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            StudentCourse studentCourse = await _context.StudentCourses
                .Include(c => c.User)
                .Include(c => c.Grades)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);
           

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
