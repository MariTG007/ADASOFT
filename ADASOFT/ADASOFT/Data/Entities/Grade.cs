using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public StudentCourse StudentCourse { get; set; }

        [Display(Name = "Porcentaje")]
        public decimal Percentage { get; set; }

        [Display(Name = "Notas")]
        public decimal Grades { get; set; }

        public decimal FinalGrade { get; set; }
        //public ICollection<FinalGrade> Grades { get; set; }
    }
}
