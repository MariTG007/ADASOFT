using ADASOFT.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Models
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public int StudentCourseId { get; set; }
        [Display(Name = "Porcentaje")]
        public decimal Percentage { get; set; }
        [Display(Name = "Nota")]
        public decimal Grades { get; set; }

        //public ICollection<FinalGradeViewModel> Grades { get; set; }
    }
}
