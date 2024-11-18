using CarBookingDAL.Configurations;
using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingDAL.Context
{
    public class CarBookingContext : DbContext
    {
        public CarBookingContext() : base()
        {
        }

        public CarBookingContext(DbContextOptions<CarBookingContext> options) : base(options)
        {
        }

        public DbSet<CarDTO> CarDTOs { get; set; }

        public DbSet<CarModelDTO> CarModelDTOs { get; set; }

        public DbSet<CarTypeDTO> CarTypeDTOs { get; set; }

        public DbSet<RentInfoDTO> RentInfoDTOs { get; set; }

        public DbSet<RentOrderDTO> RentOrderDTOs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new CarTypeEntityConfiguration())
                .ApplyConfiguration(new CarModelEntityConfiguration())
                .ApplyConfiguration(new CarEntityConfiguration())
                .ApplyConfiguration(new RentInfoEntityConfiguration())
                .ApplyConfiguration(new RentOrderEntityConfiguration());
        }
    }
}
