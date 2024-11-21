using OrderDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderDAL.Configurations
{
    internal class RentOrderEntityConfiguration : IEntityTypeConfiguration<RentOrderDTO>
    {
        public void Configure(EntityTypeBuilder<RentOrderDTO> builder)
        {
            builder.HasKey(order => order.Id);

            builder.Property(order => order.IsAcepted).IsRequired();

            builder.Property(order => order.RentInfoId).IsRequired();

            builder.HasOne(order => order.RentInfoDTO).WithMany().HasForeignKey(order => order.RentInfoId);
        }
    }
}
