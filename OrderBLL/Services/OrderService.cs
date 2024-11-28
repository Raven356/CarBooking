using OrderBLL.Interfaces;
using OrderBLL.Mappers;
using OrderBLL.Models;
using OrderDAL.Interfaces;

namespace OrderBLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<RentOrder> GetById(int orderId)
        {
            return OrderMapper.Map(await orderRepository.GetById(orderId));
        }

        public async Task<IEnumerable<RentOrder>> GetRentOrdersByUserIdAsync(int userId)
        {
            var orders = await orderRepository.GetRentOrdersByUserIdAsync(userId);

            return OrderMapper.Map(orders);
        }

        public async Task SaveOrderAsync(RentOrder rentOrder)
        {
            await orderRepository.SaveOrderAsync(OrderMapper.Map(rentOrder));
        }
    }
}
