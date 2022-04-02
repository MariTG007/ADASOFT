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

        }
    }
}