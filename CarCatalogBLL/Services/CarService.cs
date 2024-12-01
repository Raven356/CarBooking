﻿using CarBookingBLL.Interfaces;
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

        public IEnumerable<Car> GetAll()
        {
            var carsDto = carRepository.GetAll();

            var cars = CarMapper.Map(carsDto);

            return cars;
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
