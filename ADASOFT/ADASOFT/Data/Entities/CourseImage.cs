using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class CourseImage
    {
        public int Id { get; set; }

        public Course Course { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://localhost:7187/images/noimage.png"
          : $"https://adasoft.blob.core.windows.net/courses/{ImageId}";

    }
}
