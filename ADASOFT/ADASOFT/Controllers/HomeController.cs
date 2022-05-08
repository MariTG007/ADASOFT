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
            List<Course> courses = await _context.Courses
                .OrderBy(c => c.Name)
                .ToListAsync();

            HomeViewModel model = new() { Courses = courses };
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

            //string users = string.Empty;



            //foreach (ProductCategory? category in course.ProductCategories)
            //{
            //    categories += $"{category.Category.Name}, ";
            //}
            //categories = categories.Substring(0, categories.Length - 2);

            AddCourseToCartViewModel model = new()
            {
                
            //Description = course.Description,
            Id = course.Id,
                Name = course.Name,
                Users = $"{course.User.FullName}",
                Schedule = DateTime.Now,
                Date = course.Date,
                //Price = course.Price,
                //ProductImages = course.ProductImages,
                Quantity = 1,
                //Stock = course.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddCourseToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Course course = await _context.Courses.FindAsync(model.Id);
            if (course == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EnrollmentCourse temporalSale = new()
            {
                Course = course,
                Quantity = model.Quantity,
                Remarks = model.Remarks,
                User = user
            };

            _context.EnrollmentCourses.Add(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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