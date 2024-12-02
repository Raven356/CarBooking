using OrderDAL.Models;

namespace OrderDAL.Interfaces
{
    public interface IOrderRepository
    {
        public Task SaveOrderAsync(RentOrderDTO rentOrder);

        public Task<IEnumerable<RentOrderDTO>> GetRentOrdersByUserIdAsync(int userId);

        public Task<RentOrderDTO> GetById(int orderId);

        public Task EndOrderAsync(int orderId, DateTime endOrderTime);

        Task<RentOrderDTO> EditOrderAsync(int orderId, DateTime dateFrom, DateTime dateTo);
    }
}
