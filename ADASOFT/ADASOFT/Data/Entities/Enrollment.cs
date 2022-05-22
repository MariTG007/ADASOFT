using ADASOFT.Enums;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [Display(Name = "Estado Matrícula")]
        public EnrollmentStatus EnrollmentStatus { get; set; }

        public ICollection<Payment> Payments { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => Payments == null ? 0 : Payments.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => Payments == null ? 0 : Payments.Sum(sd => sd.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Payments == null ? 0 : Payments.Sum(sd => sd.Value);
    }
}
