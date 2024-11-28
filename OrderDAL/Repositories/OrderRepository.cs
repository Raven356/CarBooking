using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDAL.Context;
using OrderDAL.Interfaces;
using OrderDAL.Models;

namespace OrderDAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private OrderContext context;

        public OrderRepository(IServiceProvider serviceProvider) 
        {
            var scope = serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<OrderContext>();
        }

        public async Task<RentOrderDTO> GetById(int orderId)
        {
            var order = await context.RentOrder
                .Include(o => o.RentInfoDTO)
                .FirstAsync(o => o.Id == orderId);

            return order;
        }

        public async Task<IEnumerable<RentOrderDTO>> GetRentOrdersByUserIdAsync(int userId)
        {
            var orders = await context.RentOrder
                .Include(order => order.RentInfoDTO)
                .Where(order => order.RentInfoDTO.RentBy == userId)
                .ToListAsync();

            return orders;
        }

        public async Task SaveOrderAsync(RentOrderDTO rentOrder)
        {
            await context.AddAsync(rentOrder);

            await context.SaveChangesAsync();
        }
    }
}
