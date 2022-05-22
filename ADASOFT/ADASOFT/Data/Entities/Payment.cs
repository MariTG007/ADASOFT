using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public Enrollment Enrollment { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }
        public Course Course { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Course == null ? 0 : (decimal)Quantity * Course.Price;
    }
}
