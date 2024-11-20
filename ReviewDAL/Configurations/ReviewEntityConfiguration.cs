using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewDAL.Models;

namespace ReviewDAL.Configurations
{
    internal class ReviewEntityConfiguration : IEntityTypeConfiguration<ReviewDTO>
    {
        public void Configure(EntityTypeBuilder<ReviewDTO> builder)
        {
            builder.ToTable("Review");

            builder.HasIndex(review => review.Id);
        }
    }
}
