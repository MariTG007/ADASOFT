using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }
    }
}
