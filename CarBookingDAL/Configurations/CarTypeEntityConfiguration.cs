using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingDAL.Configurations
{
    internal class CarTypeEntityConfiguration : IEntityTypeConfiguration<CarTypeDTO>
    {
        public void Configure(EntityTypeBuilder<CarTypeDTO> builder)
        {
            builder.ToTable("CarType");

            builder.HasKey(type => type.Id);

            builder.Property(type => type.Type)
                .IsRequired();
        }
    }
}
