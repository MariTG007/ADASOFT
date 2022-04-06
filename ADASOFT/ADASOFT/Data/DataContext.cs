﻿using ADASOFT.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }



        public DbSet<Campus> Campuses { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Course> Courses{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique(); 
            modelBuilder.Entity<Campus>().HasIndex("Name", "CityId").IsUnique();
        }
    }
}


