using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class EnrollmentCourse
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

    }
}




//using System.ComponentModel.DataAnnotations;

//namespace ADASOFT.Data.Entities
//{
//    public class EnrollmentCourse
//    {
//        public int Id { get; set; }

//        [Display(Name = "Horario")]
//        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
//        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
//        public string Schedule { get; set; }

//        [Display(Name = "Descripcion")]
//        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
//        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
//        public string Description { get; set; }

//        public Course Course { get; set; }

//        public Enrollment Enrollment { get; set; }



//    }
//}
