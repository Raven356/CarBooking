using Microsoft.EntityFrameworkCore;
using ReviewDAL.Configurations;
using ReviewDAL.Models;

namespace ReviewDAL.Context
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions options) : base(options)
        {
        }

        protected ReviewContext()
        {
        }

        public DbSet<ReviewDTO> Reviews { get; set; }

        public DbSet<ChatDTO> Chats { get; set; }

        public DbSet<MessageDTO> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ChatEntityConfiguration())
                .ApplyConfiguration(new MessageEntityConfiguration())
                .ApplyConfiguration(new ReviewEntityConfiguration());
        }
    }
}
