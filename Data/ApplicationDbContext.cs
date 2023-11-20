using FlightStatusAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlightStatusAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            Console.WriteLine("DbContext is created!");
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
