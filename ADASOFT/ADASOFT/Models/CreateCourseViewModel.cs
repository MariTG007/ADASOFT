using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CreateCourseViewModel
    {


        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Horaio")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Días")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Days { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cupos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quota { get; set; }

        [Display(Name = "Lo que aprenderás")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Resume{ get; set; }

        [Display(Name = "Profesor")]
        //[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un profesor.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]


        public string UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        //public int Id { get; set; }

        //[Display(Name = "Curso")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public string Name { get; set; }

        //[Display(Name = "Descripción")]
        //[MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public string Description { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C2}")]
        //[Display(Name = "Precio")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public decimal Price { get; set; }



        ////public User UserId { get; set; }
        //[Display(Name = "Horaio")]
        ////[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        ////[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public DateTime Schedule { get; set; }

        //[Display(Name = "Días")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public String Date { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //[Display(Name = "Estudiantes")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public float Quota { get; set; }

        //[Display(Name = "Foto")]
        //public Guid ImageCourseId { get; set; }

        //[Display(Name = "Foto")]
        //public string ImageFullPath => ImageCourseId == Guid.Empty
        //    ? $"https://localhost:7187/images/noimage.png"
        //    : $"https://adasoft.blob.core.windows.net/courses/{ImageCourseId}";

        //[Display(Name = "Image")]
        //public IFormFile? ImageFile { get; set; }

    }
}
