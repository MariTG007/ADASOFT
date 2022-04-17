using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ADASOFT.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Departamento")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Name { get; set; }

        [JsonIgnore]
        public State State { get; set; }

        public ICollection<Campus> Campuses { get; set; }

        [Display(Name = "Sedes")]
        public int CampusesNumber => Campuses == null ? 0 : Campuses.Count;
    }
}


