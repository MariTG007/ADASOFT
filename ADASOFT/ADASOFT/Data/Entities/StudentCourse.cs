namespace ADASOFT.Data.Entities
{
    public class StudentCourse
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
