using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CampusViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sedes")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public int CityId { get; set; }
    }
}


