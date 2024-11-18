using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingDAL.Configurations
{
    internal class RentInfoEntityConfiguration : IEntityTypeConfiguration<RentInfoDTO>
    {
        public void Configure(EntityTypeBuilder<RentInfoDTO> builder)
        {
            builder.ToTable("RentInfo");

            builder.HasKey(info => info.Id);

            builder.Property(info => info.RentBy).IsRequired();

            builder.Property(info => info.RentToUTC).IsRequired();

            builder.Property(info => info.RentFromUTC).IsRequired();

            builder.Property(info => info.CarId).IsRequired();

            builder.HasOne(info => info.Car).WithMany().HasForeignKey(info => info.CarId);
        }
    }
}
