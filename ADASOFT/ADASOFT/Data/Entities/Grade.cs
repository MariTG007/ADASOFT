namespace ADASOFT.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public StudentCourse StudentCourse { get; set; }
        public decimal Percentage { get; set; }
        public decimal Grades { get; set; }

        //public ICollection<FinalGrade> Grades { get; set; }
    }
}
