using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Curso")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Profesor")]
        public User User { get; set; }

        [Display(Name = "Horario")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Días")]
        public String Date { get; set; }


    }
}
