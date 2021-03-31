using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users {get; set;}
        public DbSet<UserRole> UserRoles {get; set;}
        public DbSet<Vehicle> Vehicles {get; set;}
        public DbSet<VehicleDevice> VehicleDevices {get; set;}
        public DbSet<LocationDetail> LocationDetails {get; set;}
    }
}