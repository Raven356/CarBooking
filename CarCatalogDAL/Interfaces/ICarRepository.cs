using CarBookingDAL.Models;

namespace CarBookingDAL.Interfaces
{
    public interface ICarRepository
    {
        public Task<IEnumerable<CarDTO>> GetAllAsync(string? type, string? fromPrice, string? toPrice, string? model);

        public Task MakeCarBooked(int carId, int userId);

        public Task<CarDTO> GetCarByIdAsync(int carId);

        public Task MakeCarAvailable(int carId);

        Task<IEnumerable<string>> GetAllTypesAsync();

        Task<IEnumerable<string>> GetAllModelsAsync();
    }
}
