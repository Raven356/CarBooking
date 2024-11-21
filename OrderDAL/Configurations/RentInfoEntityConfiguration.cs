using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderDAL.Models;

namespace OrderDAL.Configurations
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
        }
    }
}
