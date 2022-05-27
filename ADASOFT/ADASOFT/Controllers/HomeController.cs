using ADASOFT.Common;
using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vereyon.Web;

namespace ADASOFT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IEnrollmentHelper _enrollmentHelper;
        private readonly IFlashMessage _flashMessage;

        public HomeController(ILogger<HomeController> logger, DataContext context,
            IUserHelper userHelper, IEnrollmentHelper enrollmentHelper, IFlashMessage flashMessage)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _enrollmentHelper = enrollmentHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter,
                                                string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "PriceDesc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Course> query = _context.Courses
                .Include(p => p.CourseImages)
                //.Include(p => p.ProductCategories)
                .Where(p => p.Quota > 0);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => (p.Name.ToLower().Contains(searchString.ToLower()) && p.Quota > 0));
            }
            else
            {
                query = query.Where(p => p.Quota > 0);
            }

            switch (sortOrder)
            {
                case "NameDesc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 8;

            List<Course> courses = await _context.Courses
                .Include(c => c.CourseImages)
                .Where(p => p.Quota > 0)
                .OrderBy(c => c.Name)
                .ToListAsync();

            HomeViewModel model = new() 
            {
                Courses = await query.ToListAsync(),
                Subjects = await PaginatedList<Course>.CreateAsync(query, pageNumber ?? 1, pageSize)
            };

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
                .Include(c => c.CourseImages)
                 .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            AddCourseToCartViewModel model = new()
            {
                Description = course.Description,
                Id = course.Id,
                Name = course.Name,
                Users = $"{course.User.FullName}",
                Resume = course.Resume,
                Schedule = course.Schedule,
                Days = course.Days,
                Price = course.Price,
                Quota = course.Quota,
                CourseImages = course.CourseImages,
                Quantity = 1,
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

            EnrollmentCourse enrollmentCourse = new()
            {
                Course = course,
                Quantity = model.Quantity,
                Remarks = model.Remarks,
                User = user
            };

            _context.EnrollmentCourses.Add(enrollmentCourse);
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
            if (User.IsInRole("Admin")) { 
                return RedirectToAction("NotAuthorized", "Account");

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
            ViewData["mymessage"] = "this is a message";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("NotAuthorized", "Account");

            }
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<EnrollmentCourse>? enrollmentcourses = await _context.EnrollmentCourses
                .Include(ec => ec.Course)
                .ThenInclude(c => c.CourseImages)
                .Where(ec => ec.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel model = new()
            {
                User = user,
                EnrollmentCourses = enrollmentcourses,
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.EnrollmentCourses = await _context.EnrollmentCourses
                .Include(ec => ec.Course)
                .ThenInclude(c => c.CourseImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            Response response = await _enrollmentHelper.ProcessOrderAsync(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(EnrollmentSuccess));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EnrollmentCourse enrollmentCourse = await _context.EnrollmentCourses.FindAsync(id);
            if (enrollmentCourse == null)
            {
                return NotFound();
            }

            _context.EnrollmentCourses.Remove(enrollmentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EnrollmentCourse enrollmentCourse = await _context.EnrollmentCourses.FindAsync(id);
            if (enrollmentCourse == null)
            {
                return NotFound();
            }

            EditEnrollmentCourseViewModel model = new()
            {
                Id = enrollmentCourse.Id,
                Quantity = enrollmentCourse.Quantity,
                Remarks = enrollmentCourse.Remarks,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEnrollmentCourseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    EnrollmentCourse enrollmentCourse = await _context.EnrollmentCourses.FindAsync(id);
                    enrollmentCourse.Quantity = model.Quantity;
                    enrollmentCourse.Remarks = model.Remarks;
                    _context.Update(enrollmentCourse);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
                return RedirectToAction(nameof(ShowCart));
            }
            return View(model);
        }

        [Authorize]
        public IActionResult EnrollmentSuccess()
        {
            return View();
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