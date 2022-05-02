using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CreateCourseViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Curso")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }


        //public User UserId { get; set; }
        [Display(Name = "Horaio")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Días")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Date { get; set; }
    }
}
