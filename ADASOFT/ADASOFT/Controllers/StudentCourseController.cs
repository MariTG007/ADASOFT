using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static ADASOFT.Helpers.ModalHelper;

namespace ADASOFT.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;
        public StudentCourseController(DataContext context, IUserHelper userHelper, IFlashMessage flashMessage)
        {
            _context= context;
            _userHelper= userHelper;
            _flashMessage =  flashMessage;
        }

        [Authorize(Roles = "User")]
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
            //studentCourse.FinalGrade = 0;
            //foreach (Grade? grade in studentCourse.Grades)
            //{
            //    studentCourse.FinalGrade += ((grade.Percentage/100) * grade.Grades);
            //}
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Grade grade = await _context.Grades
                .Include(c => c.StudentCourse)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (grade == null)
            {
                return NotFound();
            }
            GradeViewModel model = new()
            {
                StudentCourseId = grade.StudentCourse.Id,
                Id = grade.Id,
                Percentage = grade.Percentage,
                Grades = grade.Grades,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int id, GradeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (0 <= model.Grades && model.Grades <= 5)
                    {
                        StudentCourse studentCourse = await _context.StudentCourses
                        .Include(s => s.Grades)
                        .FirstOrDefaultAsync(u => u.Id == model.StudentCourseId);
                        studentCourse.porcentageCourse = 0;
                        Grade grade2 = await _context.Grades.FindAsync(model.Id);
                        foreach (Grade grade1 in studentCourse.Grades)
                        {
                            studentCourse.porcentageCourse += grade1.Percentage;
                        }
                        if ((studentCourse.porcentageCourse + model.Percentage -grade2.Percentage) <= 100)
                        {
                           
                            grade2.Id = model.Id;
                            grade2.Percentage = model.Percentage;
                            grade2.Grades = model.Grades;

                            _context.Update(grade2);
                            await _context.SaveChangesAsync();
                            studentCourse.FinalGrade = 0;
                            foreach (Grade? grade3 in studentCourse.Grades)
                            {
                                studentCourse.FinalGrade += ((grade3.Percentage / 100) * grade3.Grades);
                            }
                            _context.StudentCourses.Update(studentCourse);
                            await _context.SaveChangesAsync();
                            _flashMessage.Confirmation("Registro actualizado.");
                            //return RedirectToAction(nameof(DetailsGrade), new { Id = model.StudentCourseId });
                            return Json(new
                            {
                                isValid = true,
                                html = ModalHelper.RenderRazorViewToString(this, "_ViewAllGrades", studentCourse)
                            });

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "El porcentaje total no puede ser mayor q 100%");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se puede ingresar una nota que no este en el rango de 0 a 5");
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditGrade", model) });
        }

        public async Task<IActionResult> Details(int id)
        {
        
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

        [NoDirectAccess]

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
            try
            {
                if (0 <= model.Grades && model.Grades <= 5)
                {
                    StudentCourse studentCourse = await _context.StudentCourses
                    .Include(s => s.Grades)
                    .FirstOrDefaultAsync(u => u.Id == model.StudentCourseId);
                    studentCourse.porcentageCourse = 0;
                    foreach (Grade grade1 in studentCourse.Grades)
                    {
                        studentCourse.porcentageCourse += grade1.Percentage;
                    }
                    if ((studentCourse.porcentageCourse + model.Percentage) <= 100)
                    {
                        Grade grade = new()
                        {
                            StudentCourse = await _context.StudentCourses.FindAsync(model.StudentCourseId),
                            Grades = model.Grades,
                            Percentage = model.Percentage,
                        };

                        _context.Add(grade);
                        await _context.SaveChangesAsync();
                        studentCourse.FinalGrade = 0;
                        foreach (Grade? grade2 in studentCourse.Grades)
                        {
                            studentCourse.FinalGrade += ((grade2.Percentage / 100) * grade2.Grades);
                        }
                        _context.StudentCourses.Update(studentCourse);
                        await _context.SaveChangesAsync();
                        _flashMessage.Confirmation("Registro creado.");
                        return Json(new{isValid = true,html = ModalHelper.RenderRazorViewToString(this, "_ViewAllGrades", studentCourse)});

                    }
                    {
                        ModelState.AddModelError(string.Empty, "El porcentage total no puede ser mayor q 100%");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se puede ingresar una nota que no este en el rango de 0 a 5");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddGrade", model) });
            }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteGrade(int id)
        {

            Grade grade = await _context.Grades
                .Include(c => c.StudentCourse)
                .FirstOrDefaultAsync(c => c.Id == id); 
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            StudentCourse studentCourse = await _context.StudentCourses
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(u => u.Id == grade.StudentCourse.Id);
            studentCourse.FinalGrade = 0;
            foreach (Grade? grade2 in studentCourse.Grades)
            {
                studentCourse.FinalGrade += ((grade2.Percentage / 100) * grade2.Grades);
            }
            _context.StudentCourses.Update(studentCourse);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(DetailsGrade), new { Id = grade.StudentCourse.Id });
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
