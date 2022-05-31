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
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public CoursesController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses
           .Include(p => p.User)
           .Include(p => p.CourseImages)
           .ToListAsync());
        }

        [NoDirectAccess]

        public async Task<IActionResult> Create()
        {
            CourseViewModel model = new()
            {
                Users = await _combosHelper.GetComboTeachersAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "courses");
                }

                Course course = new()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Quota = model.Quota,
                    Schedule = (DateTime)model.Schedule,
                    Days = model.Days,
                    Resume = model.Resume,
                    //ImageCourseId = model.ImageCourseId,

                    User = await _context.Users.FindAsync(model.UserId)
                };

                if (imageId != Guid.Empty)
                {
                    course.CourseImages = new List<CourseImage>()
                    {
                        new CourseImage { ImageId = imageId }
                    };
                }

                try
                {
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCourses", _context.Courses
                        .Include(c => c.CourseImages)
                        .Include(c => c.User))
                    });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un curso con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Users = await _combosHelper.GetComboTeachersAsync();
               return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            CreateCourseViewModel model = new()
            {
                Users = await _combosHelper.GetComboTeachersAsync(),
                Description = course.Description,
                Id = course.Id,
                Name = course.Name,
                Resume = course.Resume,
                Schedule = (DateTime)course.Schedule,
                Days = course.Days,
                Price = course.Price,
                Quota = course.Quota,
                //Users = await _combosHelper.GetComboTeachersAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Course course = await _context.Courses.FindAsync(model.Id);
                course.Description = model.Description;
                course.Name = model.Name;
                course.Price = model.Price;
                course.Resume = model.Resume;
                course.Schedule = (DateTime)model.Schedule;
                course.Days = model.Days;
                course.Quota = model.Quota;
                course.User = await _context.Users.FindAsync(model.UserId);

                _context.Update(course);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCourses", _context.Courses
                    .Include(c => c.CourseImages)
                    .Include(c => c.User))
                });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    _flashMessage.Danger("Ya existe un curso con el mismo nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            model.Users = await _combosHelper.GetComboTeachersAsync();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
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
            return View(course);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Course course = await _context.Courses
                .Include(C => C.CourseImages)
                .Include(C => C.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            foreach (CourseImage courseImage in course.CourseImages)
            {
                await _blobHelper.DeleteBlobAsync(courseImage.ImageId, "courses");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }


        [NoDirectAccess]
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            AddCourseImageViewModel model = new()
            {
                CourseId = course.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddCourseImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "courses");
                Course course = await _context.Courses.FindAsync(model.CourseId);
                CourseImage courseImage = new()
                {
                    Course = course,
                    ImageId = imageId,
                };

                try
                {
                    _context.Add(courseImage);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Courses
                            .Include(c => c.CourseImages)
                            .Include(c => c.User)
                            .FirstOrDefaultAsync(p => p.Id == model.CourseId))
                    });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseImage courseImage = await _context.CourseImages
                .Include(ci => ci.Course)
                .FirstOrDefaultAsync(ci => ci.Id == id);
            if (courseImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(courseImage.ImageId, "courses");
            _context.CourseImages.Remove(courseImage);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = courseImage.Course.Id });
        }
    }
}