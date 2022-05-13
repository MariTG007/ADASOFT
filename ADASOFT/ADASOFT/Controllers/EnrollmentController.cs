using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using ADASOFT.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace ADASOFT.Controllers
{
    [Authorize(Roles = "Admin")]

    public class EnrollmentController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IEnrollmentHelper _enrollmentHelper;

        public EnrollmentController(DataContext context, IFlashMessage flashMessage, IEnrollmentHelper enrollmentHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _enrollmentHelper = enrollmentHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Payments)
                .ThenInclude(sd => sd.Course)
                .ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enrollment enrollment = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Payments)
                .ThenInclude(p => p.Course)
                .ThenInclude(c => c.CourseImages)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enrollment enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            if (enrollment.EnrollmentStatus != EnrollmentStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden matrícular cursos que estén en estado 'nuevo'.");
            }
            else
            {
                enrollment.EnrollmentStatus = EnrollmentStatus.Confirmado;
                _context.Enrollments.Update(enrollment);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado de la matrícula ha sido cambiado a 'confirmado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = enrollment.Id });
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enrollment enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            if (enrollment.EnrollmentStatus == EnrollmentStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar una matrícula que esté en estado 'cancelado'.");
            }
            else
            {
                await _enrollmentHelper.CancelOrderAsync(enrollment.Id);
                _flashMessage.Confirmation("El estado de la matrícula ha sido cambiado a 'cancelado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = enrollment.Id });
        }


    }
}
