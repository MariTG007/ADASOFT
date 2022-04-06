using ADASOFT.Data.Entities;

namespace ADASOFT.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)//inject Data Base
        {
            _context = context;
        }

        public async Task SeedAsync()//Similar to void
        {
            await _context.Database.EnsureCreatedAsync();//when you don't have data base
            await CheckCoursesAsync(); //Verificate if categories exist
            await CheckLocationsAsync();
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
            }

            await _context.SaveChangesAsync();
        }


        private async Task CheckCoursesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                await _context.SaveChangesAsync();
            }
        }
    }
}