using CarBookingBLL.Models;
using CarCatalogApi.Models;

namespace CarCatalogApi.Mappers
{
    public static class CarMapper
    {
        public static CarResponse Map(Car car)
        {
            return new CarResponse
            {
                CarPlate = car.CarPlate,
                Id = car.Id,
                Model = car.Model.Model,
                RentBy = car.RentBy,
                RentPrice = car.RentPrice,
                Type = car.CarType.Type,
                CarId = car.Id
            };
        }
    }
}
