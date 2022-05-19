using ADASOFT.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ADASOFT.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        //[Display(Name = "Ciudad")]
        //public City City { get; set; }

        [Display(Name = "Campus")]
        public Campus Campus { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
       
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7187/images/noimage.png"
            : $"https://adasoft.blob.core.windows.net/users/{ImageId}";

     


        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public ICollection<Attendant> Attendantes { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }



        [Display(Name = "Acudientes")]
        public int AttendantesNumber => Attendantes == null ? 0 : Attendantes.Count;

        [Display(Name = "Cursos")]
        public int EnrollmentNumber => Enrollments == null ? 0 : Enrollments.Count;

        [Display(Name = "Estudiantes")]
        public int StudentCoursesNumber => StudentCourses == null ? 0 : StudentCourses.Count;





    }
}