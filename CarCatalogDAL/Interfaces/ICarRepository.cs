using CarBookingDAL.Models;

namespace CarBookingDAL.Interfaces
{
    public interface ICarRepository
    {
        public IEnumerable<CarDTO> GetAll();

        public Task MakeCarBooked(int carId, int userId);

        public Task<CarDTO> GetCarByIdAsync(int carId);

        public Task MakeCarAvailable(int carId);
    }
}
