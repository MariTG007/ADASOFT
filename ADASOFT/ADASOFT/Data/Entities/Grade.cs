namespace ADASOFT.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public StudenCourse StudenCourse { get; set; }

        public ICollection<FinalGrade> Grades { get; set; }
    }
}
