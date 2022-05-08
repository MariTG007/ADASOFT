using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ADASOFT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, DataContext context, IUserHelper userHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            List<Course>? courses = await _context.Courses
                .OrderBy(c => c.Name)
                .ToListAsync();
            List<CoursesHomeViewModel> coursesHome = new() { new CoursesHomeViewModel() };
            int i = 1;
            foreach (Course? course in courses)
            {
                if (i == 1)
                {
                    coursesHome.LastOrDefault().Course1 = course;
                }
                if (i == 2)
                {
                    coursesHome.LastOrDefault().Course2 = course;
                }
                if (i == 3)
                {
                    coursesHome.LastOrDefault().Course3 = course;
                }
                if (i == 4)
                {
                    coursesHome.LastOrDefault().Course4 = course;
                    coursesHome.Add(new CoursesHomeViewModel());
                    i = 0;
                }
                i++;
            }

            HomeViewModel model = new() { Courses = coursesHome };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.EnrollmentCourses
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(model);

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses
                .Include(c => c.User)
                 .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Course course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EnrollmentCourse enrollmentCourse = new()
            {
                Course = course,
                Quantity = 1,
                User = user
            };

            _context.EnrollmentCourses.Add(enrollmentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

    }
}