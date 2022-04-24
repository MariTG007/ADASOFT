using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class EnrollmentCourse
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

        public Course Course { get; set; }

        public Enrollment Enrollment { get; set; }

        

    }
}
