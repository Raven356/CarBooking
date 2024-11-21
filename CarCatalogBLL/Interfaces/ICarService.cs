using CarBookingBLL.Models;

namespace CarBookingBLL.Interfaces
{
    public interface ICarService
    {
        public IEnumerable<Car> GetAll();
    }
}
