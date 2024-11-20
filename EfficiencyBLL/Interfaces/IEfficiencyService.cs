using EfficiencyBLL.Models;

namespace EfficiencyBLL.Interfaces
{
    public interface IEfficiencyService
    {
        public Task<OrderEfficiency> GetProfitsByCarAsync(int carId);
    }
}
