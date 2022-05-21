using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public int StudentCourseId { get; set; }

        public decimal Percentage { get; set; }

        public decimal Grades { get; set; }
        //public ICollection<FinalGradeViewModel> Grades { get; set; }

    }
}
