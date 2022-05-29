using ADASOFT.Data;
using ADASOFT.Enums;
using ADASOFT.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public DashboardController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.CoursesCount = _context.Courses.Count();
            ViewBag.NewOrdersCount = _context.Enrollments.Where(o => o.EnrollmentStatus == EnrollmentStatus.Nuevo).Count();
            ViewBag.ConfirmedOrdersCount = _context.Enrollments.Where(o => o.EnrollmentStatus == EnrollmentStatus.Confirmado).Count();

            return View(await _context.EnrollmentCourses
                    .Include(u => u.User)
                    .Include(p => p.Course).ToListAsync());
        }
    }
}
