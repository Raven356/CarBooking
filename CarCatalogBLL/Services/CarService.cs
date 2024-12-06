using CarBookingBLL.Interfaces;
using CarBookingBLL.Mappers;
using CarBookingBLL.Models;
using CarBookingDAL.Interfaces;

namespace CarBookingBLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository carRepository;

        public CarService(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllAsync(string? type, string? fromPrice, string? toPrice, string? model)
        {
            var carsDto = await carRepository.GetAllAsync(type, fromPrice, toPrice, model);

            var cars = CarMapper.Map(carsDto);

            return cars;
        }

        public async Task<IEnumerable<string>> GetAllModelsAsync()
        {
            return await carRepository.GetAllModelsAsync();
        }

        public async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            return await carRepository.GetAllTypesAsync();
        }

        public async Task<Car> GetByIdAsync(int carId)
        {
            return CarMapper.Map(await carRepository.GetCarByIdAsync(carId));
        }

        public async Task MakeCarAvailable(int carId)
        {
            await carRepository.MakeCarAvailable(carId);
        }

        public async Task MakeCarBooked(int carId, int userId)
        {
            await carRepository.MakeCarBooked(carId, userId);
        }
    }
}
