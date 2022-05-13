using ADASOFT.Common;
using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using ADASOFT.Models;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Helpers
{
    public class EnrollmentHelper : IEnrollmentHelper
    {
        private readonly DataContext _context;

        public EnrollmentHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Response> ProcessOrderAsync(ShowCartViewModel model)
        {
            Response response = await CheckInventoryAsync(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            Enrollment enrollment = new()
            {
                Date = DateTime.UtcNow,
                User = model.User,
                Remarks = model.Remarks,
                Payments = new List<Payment>(),
                EnrollmentStatus = EnrollmentStatus.Nuevo
            };

            foreach (EnrollmentCourse? item in model.EnrollmentCourses)
            {
                enrollment.Payments.Add(new Payment
                {
                    Course = item.Course,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                });

                Course course = await _context.Courses.FindAsync(item.Course.Id);
                if (course != null)
                {
                    course.Quota -= item.Quantity;
                    _context.Courses.Update(course);
                }

                _context.EnrollmentCourses.Remove(item);
            }

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<Response> CheckInventoryAsync(ShowCartViewModel model)
        {
            Response response = new() { IsSuccess = true };
            foreach (EnrollmentCourse? item in model.EnrollmentCourses)
            {
                Course course = await _context.Courses.FindAsync(item.Course.Id);
                if (course == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El producto {item.Course.Name}, ya no está disponible";
                    return response;
                }
                if (course.Quota < item.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Lo sentimos no tenemos cupos suficientes del curso {item.Course.Name}, para tomar su matrícula sustituirlo por otro.";
                    return response;
                }
            }
            return response;
        }
        public async Task<Response> CancelOrderAsync(int id)
        {
            Enrollment enrollment = await _context.Enrollments
                .Include(e => e.Payments)
                .ThenInclude(p => p.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            foreach (Payment payment in enrollment.Payments)
            {
                Course course = await _context.Courses.FindAsync(payment.Course.Id);
                if (course != null)
                {
                    course.Quota += payment.Quantity;
                }
            }

            enrollment.EnrollmentStatus = EnrollmentStatus.Cancelado;
            await _context.SaveChangesAsync();
            return new Response { IsSuccess = true };
        }



    }
}
