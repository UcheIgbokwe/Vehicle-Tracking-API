using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                    .HasMany(u => u.Vehicles)
                    .WithOne(u => u.User);
        }

        public DbSet<Vehicle> Vehicles {get; set;}
        public DbSet<VehicleDevice> VehicleDevices {get; set;}
        public DbSet<LocationDetail> LocationDetails {get; set;}
    }
}