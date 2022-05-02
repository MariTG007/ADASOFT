namespace ADASOFT.Data.Entities
{
    public class CourseUser
    {
        public int Id { get; set; }

        public Course Course { get; set; }

        public User User { get; set; }
    }
}
