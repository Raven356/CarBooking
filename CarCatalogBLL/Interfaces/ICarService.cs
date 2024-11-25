using CarBookingBLL.Models;

namespace CarBookingBLL.Interfaces
{
    public interface ICarService
    {
        public IEnumerable<Car> GetAll();

        public Task MakeCarBooked(int carId, int userId);
    }
}
