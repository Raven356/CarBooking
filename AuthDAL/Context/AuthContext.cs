using AuthDAL.Configurations;
using AuthDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDAL.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions options) : base(options)
        {
        }

        public AuthContext() : base() { }

        public DbSet<UserDTO> Users { get; set; }

        public DbSet<TokenDTO> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new UserEntityConfiguration())
                .ApplyConfiguration(new TokenEntityConfiguration());
        }
    }
}
