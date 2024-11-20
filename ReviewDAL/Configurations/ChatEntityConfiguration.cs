using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewDAL.Models;

namespace ReviewDAL.Configurations
{
    internal class ChatEntityConfiguration : IEntityTypeConfiguration<ChatDTO>
    {
        public void Configure(EntityTypeBuilder<ChatDTO> builder)
        {
            builder.ToTable("Chat");

            builder.HasKey(x => x.Id);

            builder
                .HasIndex(chat => chat.ChatGuid, "Chat_ChatGuid_Unique")
                .IsUnique();
        }
    }
}
