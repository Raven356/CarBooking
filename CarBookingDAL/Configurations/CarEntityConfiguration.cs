using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingDAL.Configurations
{
    internal class CarEntityConfiguration : IEntityTypeConfiguration<CarDTO>
    {
        public void Configure(EntityTypeBuilder<CarDTO> builder)
        {
            builder.ToTable("Car");

            builder.HasKey(car => car.Id);

            builder.Property(car => car.TypeId).IsRequired();

            builder.Property(car => car.CarPlate).IsRequired();

            builder.HasIndex(car => car.CarPlate, "Car_CarPlate_Unique").IsUnique();

            builder.Property(car => car.ModelId).IsRequired();

            builder.Property(car => car.RentPrice).IsRequired();

            builder.HasOne(car => car.Model)
                .WithMany()
                .HasForeignKey(car => car.ModelId);

            builder.HasOne(car => car.CarType)
                .WithMany()
                .HasForeignKey(car => car.TypeId);
        }
    }
}
