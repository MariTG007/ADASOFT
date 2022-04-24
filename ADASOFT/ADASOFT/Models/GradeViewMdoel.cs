using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class GradeViewMdoel
    {
        public int Id { get; set; }
        public int StudenCourseId { get; set; }

        public ICollection<FinalGradeViewModel> Grades { get; set; }
    }
}
