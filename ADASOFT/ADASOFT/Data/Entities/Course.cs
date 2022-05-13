using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADASOFT.Data.Entities
{
    public class Course
    {

        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

       [Display(Name = "Horario")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Días")]
        public String? Date { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Estudiantes")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public int QuotaNumber => Campuses == null ? 0 : Campuses.Count;
        public int Quota { get; set; }

        [Display(Name = "Profesor")]
        public User? User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Lo que aprenderás")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? Resume { get; set; }


        public ICollection<CourseImage> CourseImages { get; set; }

        [Display(Name = "Fotos")]
        public int ImagesNumber => CourseImages == null ? 0 : CourseImages.Count;

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => CourseImages == null || CourseImages.Count == 0
            ? $"https://localhost:7187/images/noimage.png"
            : CourseImages.FirstOrDefault().ImageFullPath;
    

    }
}
