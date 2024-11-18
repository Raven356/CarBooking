using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingDAL.Configurations
{
    internal class CarModelEntityConfiguration : IEntityTypeConfiguration<CarModelDTO>
    {
        public void Configure(EntityTypeBuilder<CarModelDTO> builder)
        {
            builder.ToTable("CarModel");

            builder.HasKey(model => model.Id);

            builder.Property(model => model.Model).IsRequired();
        }
    }
}
