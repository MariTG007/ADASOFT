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

        private async Task CheckCoursesAsync()
        {
            if (!_context.Courses.Any())
            {
                await AddCourseAsync("Pintura", "Pintura Expresionista", 270000M, "Lunes-Miércoles", "", 20, new List<string>() { "elgrito.png" }, "4040");
                await AddCourseAsync("Baile", "Merengue,porro,salsa,Bachata,break dance", 800000M, "Lunes-Miércoles", "", 20, new List<string>() { "elgrito.png" }, "1010");

                //await AddCourseAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "Adidas_superstar.png" });
                //await AddCourseAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "airpos.png", "airpos2.png" });
                //await AddCourseAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audifonos_bose.png" });
                //await AddCourseAsync("Bicicleta Ribble", 12000000M, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
                //await AddCourseAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() { "camisa_cuadros.png" });
                //await AddCourseAsync("Casco Bicicleta", 820000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bicicleta.png", "casco.png" });
                //await AddCourseAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
                //await AddCourseAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                //await AddCourseAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac_book_pro.png" });
                //await AddCourseAsync("Mancuernas", 370000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
                //await AddCourseAsync("Mascarilla Cara", 26000M, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.png" });
                //await AddCourseAsync("New Balance 530", 180000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance530.png" });
                //await AddCourseAsync("New Balance 565", 179000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance565.png" });
                //await AddCourseAsync("Nike Air", 233000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air.png" })
                //await AddCourseAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_zoom.png" });
                //await AddCourseAsync("Buso Adidas Mujer", 134000M, 12F, new List<string>() { "Ropa", "Deportes" }, new List<string>() { "buso_adidas.png" });
                //await AddCourseAsync("Suplemento Boots Original", 15600M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "Boost_Original.png" });
                //await AddCourseAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
                //await AddCourseAsync("Arnes Mascota", 25000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.png" });
                //await AddCourseAsync("Cama Mascota", 99000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama_mascota.png" });
                //await AddCourseAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "teclado_gamer.png" });
                //await AddCourseAsync("Silla Gamer", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "silla_gamer.png" });
                //await AddCourseAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }

        }
        private async Task AddCourseAsync(
            string name, 
            string description, 
            decimal price, 
            string date, 
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
                Date = date,
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