using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using ADASOFT.Helpers;

namespace ADASOFT.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()//Similar to void
        {
            await _context.Database.EnsureCreatedAsync();//when you don't have data base
            await CheckLocationsAsync();
            await CheckRolesAsync(); // check if roles is null
            await CheckUserAsync("1010", "Andres", "Perez", "andres@yopmail.com", "314 587 9758", "Carrera 50b # 47-58", UserType.Admin);
            await CheckUserAsync("2020", "Mariana", "Trejos", "mariana@yopmail.com", "322 311 2031", "Calle Luna Calle Sol", UserType.User);
           
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Campus = _context.Campuses.FirstOrDefault(), //aca es donde se cambia a campus
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);


            }

            return user;
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