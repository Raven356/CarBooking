using CarBookingDAL.Configurations;
using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingDAL.Context
{
    public class CarCatalogContext : DbContext
    {
        public CarCatalogContext() : base()
        {
        }

        public CarCatalogContext(DbContextOptions<CarCatalogContext> options) : base(options)
        {
        }

        public DbSet<CarDTO> CarDTOs { get; set; }

        public DbSet<CarModelDTO> CarModelDTOs { get; set; }

        public DbSet<CarTypeDTO> CarTypeDTOs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new CarTypeEntityConfiguration())
                .ApplyConfiguration(new CarModelEntityConfiguration())
                .ApplyConfiguration(new CarEntityConfiguration());
        }
    }
}
