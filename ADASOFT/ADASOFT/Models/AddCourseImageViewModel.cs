using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class AddCourseImageViewModel
    {
        public int CourseId { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
    }
}
