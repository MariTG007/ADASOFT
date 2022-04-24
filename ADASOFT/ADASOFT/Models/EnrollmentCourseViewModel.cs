using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class EnrollmentCourseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Horario")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Schedule { get; set; }

        [Display(Name = "Descripcion")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        public int CourseId { get; set; }

        public int EnrollmentId { get; set; }
    }
}
