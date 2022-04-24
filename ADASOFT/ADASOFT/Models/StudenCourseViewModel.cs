using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class StudenCourseViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CourseId { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
