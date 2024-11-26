using CarBookingUI.Models;
using CarBookingUI.Models.Responses.CarResponses;

namespace CarBookingUI.Mappers
{
    internal static class CarMapper
    {
        public static Car Map(CarResponse carResponse)
        {
            return new Car
            {
                Id = carResponse.Id,
                ImageSource = "login_icon.png",
                Name = $"{carResponse.CarType.Type} {carResponse.Model.Model} {carResponse.CarPlate}",
                Price = carResponse.RentPrice.ToString(),
            };
        }

        public static IEnumerable<Car> Map(IEnumerable<CarResponse> carsResponse)
        {
            return carsResponse.Select(Map);
        }
    }
}
