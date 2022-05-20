using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class StudentCourse
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }

        public ICollection<Grade> Grades { get; set; }

        [Display(Name = "Notas")]
        public int GradesNumber => Grades == null ? 0 : Grades.Count;
    }
}
