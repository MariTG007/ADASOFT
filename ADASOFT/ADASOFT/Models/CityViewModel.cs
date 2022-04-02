using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ciudades")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Name { get; set; }


        public int StateId { get; set; }
    }
}


