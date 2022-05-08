using ADASOFT.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public ICollection<EnrollmentCourse> EnrollmentCourses { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => EnrollmentCourses == null ? 0 : EnrollmentCourses.Sum(ts => ts.Quantity);

        //[DisplayFormat(DataFormatString = "{0:C2}")]
        //[Display(Name = "Valor")]
        //public decimal Value => EnrollmentCourses == null ? 0 : EnrollmentCourses.Sum(ts => ts.Value);
    }

}
