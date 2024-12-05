using CarBookingBLL.Models;

namespace CarBookingBLL.Interfaces
{
    public interface ICarService
    {
        public Task<IEnumerable<Car>> GetAllAsymc(string? type, string? fromPrice, string? toPrice, string? model);

        public Task MakeCarBooked(int carId, int userId);

        public Task<Car> GetByIdAsync(int carId);

        public Task MakeCarAvailable(int carId);

        public Task<IEnumerable<string>> GetAllTypesAsync();

        public Task<IEnumerable<string>> GetAllModelsAsync();
    }
}
