namespace ADASOFT.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public StudentCourse StudentCourse { get; set; }

        public ICollection<FinalGrade> Grades { get; set; }
    }
}
