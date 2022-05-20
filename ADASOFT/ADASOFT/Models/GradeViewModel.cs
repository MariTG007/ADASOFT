using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public int StudentCourseId { get; set; }

        public float Percentage { get; set; }

        public float Grades { get; set; }
        //public ICollection<FinalGradeViewModel> Grades { get; set; }

    }
}
