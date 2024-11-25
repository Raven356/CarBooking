using CarBookingDAL.Models;

namespace CarBookingDAL.Interfaces
{
    public interface ICarRepository
    {
        public IEnumerable<CarDTO> GetAll();

        public Task MakeCarBooked(int carId, int userId);
    }
}
