using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Departamento")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
        [Display(Name = "Campuses")]
        public int CampusesNumber => Cities == null ? 0 : Cities.Sum(s => s.CampusesNumber);

    }
}





