using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewDAL.Models;

namespace ReviewDAL.Configurations
{
    internal class MessageEntityConfiguration : IEntityTypeConfiguration<MessageDTO>
    {
        public void Configure(EntityTypeBuilder<MessageDTO> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(message => message.Id);

            builder.HasOne(message => message.Chat)
                .WithMany()
                .HasForeignKey(message => message.ChatId);
        }
    }
}
