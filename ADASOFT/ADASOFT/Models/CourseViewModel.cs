using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class CourseViewModel : CreateCourseViewModel
    {

        //[Display(Name = "Profesor")]
        //[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un profesor.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]

        
        public string UserId { get; set; }
        
        public IEnumerable<SelectListItem> Users { get; set; }
        //public User UserId { get; set; }
       


    }
}
