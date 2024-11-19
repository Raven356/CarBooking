using AuthDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDAL.Configurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<UserDTO>
    {
        public void Configure(EntityTypeBuilder<UserDTO> builder)
        {
            builder.ToTable("User");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Phone).HasMaxLength(50);
        }
    }
}
