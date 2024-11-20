using EfficiencyBLL.Interfaces;
using EfficiencyBLL.Models;

namespace EfficiencyBLL.Services
{
    public class EfficiencyService : IEfficiencyService
    {
        public async Task<OrderEfficiency> GetProfitsByCarAsync(int carId)
        {
            return new OrderEfficiency { AmountOfOrders = 12, CarId = 1, Revenue = 2000 };
        }
    }
}
