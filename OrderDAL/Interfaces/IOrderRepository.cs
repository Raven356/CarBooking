using OrderDAL.Models;

namespace OrderDAL.Interfaces
{
    public interface IOrderRepository
    {
        public Task SaveOrderAsync(RentOrderDTO rentOrder);

        public Task<IEnumerable<RentOrderDTO>> GetRentOrdersByUserIdAsync(int userId);
    }
}
