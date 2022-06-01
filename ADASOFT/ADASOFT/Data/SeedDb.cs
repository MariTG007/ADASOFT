using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using ADASOFT.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()//Similar to void
        {
            await _context.Database.EnsureCreatedAsync();//when you don't have data base
            await CheckLocationsAsync();
            await CheckRolesAsync(); // check if roles is null
            await CheckUserAsync("1010", "Andres", "Perez", "andres@yopmail.com", "314 587 9758", "Carrera 50b # 47-58","Andres.png",UserType.Admin);
            await CheckUserAsync("3030", "Carlos", "Santana", "carlos@yopmail.com", "300 685 9318", "Calle 40 # 51", "carlitos.jpg", UserType.Admin);
            await CheckUserAsync("4040", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "zulu.png", UserType.Admin);
            await CheckUserAsync("5050", "Halle", "Berry", "halle@yopmail.com", "304 589 6320", "Ciudad Gótica", "halleberry.jpg", UserType.Admin);
            await CheckUserAsync("6060", "Tom", "Hanks", "tommy@yopmail.com", "301 149 5700", "Villa Verde", "tomhanks.jpg", UserType.Admin);
            await CheckUserAsync("7070", "Viola", "Davis", "davis@yopmail.com", "300 038 9214", "Villa Hermosa", "violadavis.jpg", UserType.Admin);
       
            await CheckUserAsync("2020", "Mariana", "Trejos", "mariana@yopmail.com", "300 658 6231", "Calle de las sombrillas","MarianaCarnet.jpg" ,UserType.User);
            await CheckUserAsync("8080", "Julio", "Jimenez", "julio@yopmail.com", "300 806 1865", "Carrera 76A # 32-73", "Julio.jpeg", UserType.User);
            await CheckUserAsync("9090", "Ana", "de Armas", "ana@yopmail.com", "322 311 2031", "Carrera 50A # 12-73", "anadearmas.jpg", UserType.User);
            await CheckUserAsync("10010","Leonardo", "DiCaprio", "leonardo@yopmail.com", "300 311 2031", "Carrera 31 Terranova", "leonardodicaprio.jpg", UserType.User);
            await CheckUserAsync("1110", "Brad", "Pitt", "brad@yopmail.com", "314 806 2031", "Av. El Poblado # 29 ", "bradpitt.jpg", UserType.User);
            await CheckUserAsync("1210", "Scarlett", "Johansson", "scarlett@yopmail.com", "322 589 311 ", "Cra. 50 #38-320", "scarlettjohansson.jpg", UserType.User);
            await CheckUserAsync("1310", "Beatrix", "Kiddo", "lanovia@yopmail.com", "301 312 9041", "Cl. 67 # 53", "beatrixkiddo.jpg", UserType.User);
            await CheckUserAsync("1410", "Phil", "Jones", "jonesta@yopmail.com", "301 356 6281", "Carrera 48A # 23", "philjones.png", UserType.User);
            await CheckUserAsync("1510", "Johnny", "Depp", "sparrow@yopmail.com", "311 696 1241", "calle de salto 21", "johnnydepp.jpg", UserType.User);
            await CheckAttendantsAsync();
            await CheckCoursesAsync();
        }

       

        private async Task<User> CheckUserAsync(
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           string address,
           string image,
           UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\Images\\users\\{image}", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Campus = _context.Campuses.FirstOrDefault(), //campus change
                    UserType = userType,
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }
        private async Task CheckAttendantsAsync()
        {
            if (!_context.Courses.Any())
            {
                await AddAttendantAsync("1001", "Andres", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "andres1@yopmail.com", "9090");
                await AddAttendantAsync("2002", "Juan", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "juan1@yopmail.com", "8080");
                await AddAttendantAsync("3003", "Julio", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "julio1@yopmail.com", "2020");
                await AddAttendantAsync("4004", "Sebastian", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "sebastian1@yopmail.com", "10010");
                await AddAttendantAsync("5005", "Mariana", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "mariana1@yopmail.com", "1110");
                await AddAttendantAsync("6006", "Claudia", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "claudia1@yopmail.com", "1210");
                await AddAttendantAsync("7007", "Ana", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "ana1@yopmail.com", "1310");
                await AddAttendantAsync("8008", "Cristian", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "cristian1@yopmail.com", "1410");
                await AddAttendantAsync("9009", "Luisa", "Perez", "Carrera 50b # 47-58", "200 6589", "314 587 9758", "luisa1@yopmail.com", "1510");

            }
        }
        private async Task AddAttendantAsync(
            string document,
            string name,
            string lastname,
            string adress,
            string phone,
            string cellphone,
            string email,
            string studentDocument)
        {
            Attendant attendant = new()
            {
                Document = document,
                FirstName = name,
                LastName = lastname,
                Address = adress,
                Phone = phone,
                Cellphone = cellphone,
                Email = email,
                User = await _context.Users.FirstOrDefaultAsync(u => u.Document == studentDocument),
                
            };

            
            _context.Attendantes.Add(attendant);
        }

        private async Task CheckCoursesAsync()
        {
            if (!_context.Courses.Any())
            {
                await AddCourseAsync("Pintura Expresionista", "Pintura Expresionista", 270000M, "Lunes-Miércoles", "Aprender a profundizar en las diferentes técnicas pictórica dándole profundidad y madurez al trabajo personal.Permitiendo así la capacidad de creación y expresión artística.", 12, new List<string>() { "elgrito.png" }, "4040");
                await AddCourseAsync("Baile Urbano", "Estilo Urbano", 200000M, "Lunes-Miércoles", "Explorando posibilidades de movimiento,profundizando en el arte, potenciando la creatividad.", 20, new List<string>() { "breakdance.jpeg" }, "1010");
                await AddCourseAsync("Baile Regional", "Regional Colombiano", 200000M, "Martes-Jueves", "Explorando posibilidades de movimiento,profundizando en el arte, potenciando la creatividad.", 20, new List<string>() { "baile.jpeg" }, "6060");
                await AddCourseAsync("Literatura", "Narrativa, lírica y dramática", 320000M, "Lunes-Martes", "Desarrollar la creatividad, la inteligencia emocional y la sensibilidad.", 8, new List<string>() { "escritura.jpeg" }, "3030");
                await AddCourseAsync("Música", "Música clásica, de consumo, tradicional y étnica", 800000M, "Miércoles-Jueves", "Desarrollar la creatividad, la inteligencia emocional y la sensibilidad. A su vez, la práctica de ésta disciplina estimula las capacidades motoras,cognitivas, afectivas, introspectivas, perceptivas de las personas. Como también fomenta el trabajo en equipo y la valoración individual.", 10, new List<string>() { "musica.jpeg" }, "4040");
                await AddCourseAsync("Cine", "Géneros cinematográficos", 429000M, "Jueves-Viernes", "Desarrollar la creatividad, la inteligencia emocional y la sensibilidad.", 25, new List<string>() { "cine.jpeg" }, "5050");
                await AddCourseAsync("Pintura Abstracta", "Pintura Abstracta", 215000M, "Martes-Jueves", "Aprender a profundizar en las diferentes técnicas pictórica dándole profundidad y madurez al trabajo personal.Permitiendo así la capacidad de creación y expresión artística.", 12, new List<string>() { "pintura.jpeg" }, "1010");
                await AddCourseAsync("Escultura", "Creación de formas y figuras en 3D", 800000M, "Miércoles-Viernes", "Descubrir los volúmenes reales de los objetos y las personas.", 10, new List<string>() { "escultura.jpeg" }, "7070");
                await AddCourseAsync("Escultura Griega", "Creación de formas y figuras en 3D", 800000M, "Miércoles-Viernes", "Descubrir los volúmenes reales de los objetos y las personas.", 10, new List<string>() { "escultura2.jpeg" }, "7070");
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddCourseAsync(
            string name, 
            string description, 
            decimal price, 
            string days, 
            string resume, 
            int Quota, 
            List<string> images, 
            string teacherDocument)
        {
            Course course = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Schedule = DateTime.UtcNow.AddMonths(2),
                Days = days,
                Resume = resume,
                Quota = Quota,
                User = await _context.Users.FirstOrDefaultAsync(u => u.Document == teacherDocument),
                CourseImages = new List<CourseImage>()
            };

            foreach (string? image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\Images\\courses\\{image}", "courses");
                course.CourseImages.Add(new CourseImage { ImageId = imageId });
            }
            _context.Courses.Add(course);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            //await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString()); //User Types are only in classes usertype and seedDb 
        }

        private async Task CheckLocationsAsync()
        {
            if (!_context.States.Any())
            {
                _context.States.Add(new State
                {
                    Name = "Antioquia",
                    Cities = new List<City>()
                    {
                        new City()
                        {
                            Name = "Medellín",
                            Campuses = new List<Campus>() {
                                new Campus() { Name = "Robledo" },
                                new Campus() { Name = "Fraternidad" },
                                new Campus() { Name = "Floresta" },
                                new Campus() { Name = "Castilla" },
                            }
                        },
                        new City()
                        {
                            Name = "Envigado",
                            Campuses = new List<Campus>() {
                                new Campus() { Name = "Las Palmas" },
                                new Campus() { Name = "Área Urbana" },
                            }
                        },
                    }
                });
                _context.States.Add(new State
                {
                    Name = "Bogotá",
                    Cities = new List<City>()
                    {
                        new City()
                        {
                            Name = "Santa fe",
                            Campuses = new List<Campus>() {
                                new Campus() { Name = "Sagrado Corazón" },
                                new Campus() { Name = "La Macarena" },
                                new Campus() { Name = "Lourdes" },
                            }
                        },
                        new City()
                        {
                            Name = "Chapinero",
                            Campuses = new List<Campus>() {
                                new Campus() { Name = "Chicó Lago" },
                                new Campus() { Name = "El Refugio" },
                                new Campus() { Name = "Pardo Rubio" },
                            }
                        },
                    }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}