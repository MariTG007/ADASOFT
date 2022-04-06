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
        public DateTime Schedule { get; set; }
        public String Date { get; set; }


    }
}
