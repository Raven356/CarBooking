using CarBookingBLL.Models;
using CarBookingDAL.Models;

namespace CarBookingBLL.Mappers
{
    public static class CarMapper
    {
        public static Car Map(CarDTO car)
        {
            return new Car
            {
                CarPlate = car.CarPlate,
                Id = car.Id,
                CarType = Map(car.CarType),
                Model = Map(car.Model),
                RentPrice = car.RentPrice,
                RentBy = car.RentBy,
                Image = car.Image,
            };
        }

        public static IEnumerable<Car> Map(IEnumerable<CarDTO> cars)
        {
            return cars.Select(Map);
        }

        public static CarType Map(CarTypeDTO carType)
        {
            return new CarType
            {
                Id = carType.Id,
                Type = carType.Type,
            };
        }

        public static CarModel Map(CarModelDTO carModelDTO)
        {
            return new CarModel
            {
                Id = carModelDTO.Id,
                Model = carModelDTO.Model
            };
        }
    }
}
