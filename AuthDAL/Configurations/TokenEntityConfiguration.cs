using AuthDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDAL.Configurations
{
    internal class TokenEntityConfiguration : IEntityTypeConfiguration<TokenDTO>
    {
        public void Configure(EntityTypeBuilder<TokenDTO> builder)
        {
            builder.ToTable("Token");

            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);
        }
    }
}
