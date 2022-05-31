using ADASOFT.Common;
using ADASOFT.Data.Entities;

namespace ADASOFT.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Course> Courses { get; set; }
        public float Quantity { get; set; }
    }
}
