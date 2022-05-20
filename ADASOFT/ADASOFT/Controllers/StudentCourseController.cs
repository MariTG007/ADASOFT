using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
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
                .ThenInclude(s => s.Grades)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


    }
}
