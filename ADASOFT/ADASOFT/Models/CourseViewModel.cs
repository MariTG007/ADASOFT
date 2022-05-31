using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CourseViewModel : CreateCourseViewModel
    {
        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
    }
}
