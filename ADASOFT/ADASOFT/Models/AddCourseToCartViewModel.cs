using ADASOFT.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class AddCourseToCartViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Curso")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Profesor")]
        public string Users { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Horario")]
      
        public DateTime Schedule { get; set; }

        [Display(Name = "Días")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Days { get; set; }

        //[Display(Name = "Nombre")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public string Name { get; set; }

        

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cupos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quota { get; set; }

        //[Display(Name = "Categorías")]
        //public string Categories { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Resume { get; set; }


        public ICollection<CourseImage> CourseImages { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageCourseId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageCourseId == Guid.Empty
            ? $"https://localhost:7187/images/noimage.png"
            : $"https://adasoft.blob.core.windows.net/courses/{ImageCourseId}";

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

    }
}
