using Microsoft.EntityFrameworkCore;
using OrderDAL.Configurations;
using OrderDAL.Models;

namespace OrderDAL.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RentInfoDTO> Rent { get; set; }

        public DbSet<RentOrderDTO> RentOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new RentInfoEntityConfiguration())
                .ApplyConfiguration(new RentOrderEntityConfiguration());
        }
    }
}
