using CarBookingDAL.Models;

namespace CarBookingDAL.Interfaces
{
    public interface ICarRepository
    {
        public IEnumerable<CarDTO> GetAll();
    }
}
