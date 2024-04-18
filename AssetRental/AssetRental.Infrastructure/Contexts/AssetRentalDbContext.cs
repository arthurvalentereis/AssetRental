

using Microsoft.EntityFrameworkCore;
using AssetRental.Domain.Entities;

namespace AssetRental.Infrastructure.Contexts
{
    public class AssetRentalDbContext : DbContext
    {
        public DbSet<Motorcycle> Motorcycle { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<RentalPlan> RentalPlan { get; set; }
        public DbSet<Driver> Driver { get; set; }
  
        public AssetRentalDbContext(DbContextOptions<AssetRentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
