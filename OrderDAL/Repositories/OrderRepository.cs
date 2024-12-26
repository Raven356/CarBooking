using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDAL.Context;
using OrderDAL.Interfaces;
using OrderDAL.Models;

namespace OrderDAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IServiceProvider serviceProvider;

        public OrderRepository(IServiceProvider serviceProvider) 
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<RentOrderDTO> EditOrderAsync(int orderId, DateTime dateFrom, DateTime dateTo)
        {
            using var context = CreateContext();

            var order = await context.RentOrder
                .Include(order => order.RentInfoDTO)
                .FirstAsync(order => order.Id == orderId);

            if (order == null)
            {
                throw new ArgumentNullException($"Order with id: {orderId} not found!");
            }

            order.RentInfoDTO.RentFromUTC = dateFrom;
            order.RentInfoDTO.RentToUTC = dateTo;

            context.Update(order);

            await context.SaveChangesAsync();

            return order;
        }

        public async Task EndOrderAsync(int orderId, DateTime endOrderTime)
        {
            using var context = CreateContext();

            var order = await context.RentOrder
                .Include(order => order.RentInfoDTO)
                .FirstAsync(order => order.Id == orderId);

            if (order == null)
            {
                throw new ArgumentNullException($"Order with id: {orderId} not found!");
            }

            order.RentInfoDTO.RentFinished = endOrderTime;
            context.Update(order);

            await context.SaveChangesAsync();
        }

        public async Task<RentOrderDTO> GetById(int orderId)
        {
            using var context = CreateContext();

            var order = await context.RentOrder
                .Include(o => o.RentInfoDTO)
                .FirstAsync(o => o.Id == orderId);

            return order;
        }

        public async Task<IEnumerable<RentOrderDTO>> GetByUserId(int userId)
        {
            using var context = CreateContext();

            var orders = await context.RentOrder
                .Include(o => o.RentInfoDTO)
                .Where(o => o.RentInfoDTO.RentBy == userId)
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<RentOrderDTO>> GetRentOrdersByUserIdAsync(int userId)
        {
            using var context = CreateContext();

            var orders = await context.RentOrder
                .Include(order => order.RentInfoDTO)
                .Where(order => order.RentInfoDTO.RentBy == userId)
                .ToListAsync();

            return orders;
        }

        public async Task SaveOrderAsync(RentOrderDTO rentOrder)
        {
            using var context = CreateContext();

            await context.AddAsync(rentOrder);

            await context.SaveChangesAsync();
        }

        private OrderContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();

            return context;
        }
    }
}
