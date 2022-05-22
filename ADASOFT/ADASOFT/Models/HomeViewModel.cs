using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class HomeViewModel
    {
        public ICollection<Course> Courses { get; set; }
        public float Quantity { get; set; }
    }
}
