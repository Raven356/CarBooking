using OrderBLL.Models;

namespace OrderBLL.Interfaces
{
    public interface IOrderService
    {
        public Task SaveOrderAsync(RentOrder rentOrder);

        public Task<IEnumerable<RentOrder>> GetRentOrdersByUserIdAsync(int userId);
        Task<RentOrder> GetById(int orderId);
    }
}
