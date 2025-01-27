﻿using CarBookingDAL.Context;
using CarBookingDAL.Interfaces;
using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingDAL.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IServiceProvider serviceProvider;

        public CarRepository(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<CarDTO>> GetAllAsync(string? type, string? fromPrice, string? toPrice, string? model)
        {
            using var context = CreateContext();

            var cars = await context.CarDTOs
                .Where(car => car.RentBy == null)
                .Include(car => car.CarType)
                .Include(car => car.Model)
                .ToListAsync();

            if (!string.IsNullOrEmpty(type))
            {
                cars = cars.Where(car => car.CarType.Type == type)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(fromPrice)) 
            {
                cars = cars.Where(car => car.RentPrice >= (fromPrice == null ? 0 : double.Parse(fromPrice)))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(toPrice))
            {
                cars = cars.Where(car => car.RentPrice <= (toPrice == null ? 0 : double.Parse(toPrice)))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(model))
            {
                cars = cars.Where(car => car.Model.Model == model)
                    .ToList();
            }

            return cars;
        }

        public async Task<IEnumerable<string>> GetAllModelsAsync()
        {
            using var context = CreateContext();

            return await context.CarModelDTOs
                .Select(model => model.Model)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            using var context = CreateContext();

            return await context.CarTypeDTOs
                .Select(type => type.Type)
                .ToListAsync();
        }

        public async Task<CarDTO> GetCarByIdAsync(int carId)
        {
            using var context = CreateContext();

            var car = await context.CarDTOs
                .Include(car => car.CarType)
                .Include(car => car.Model)
                .FirstAsync(car => car.Id == carId);

            return car;
        }

        public async Task MakeCarAvailable(int carId)
        {
            using var context = CreateContext();

            var existingCar = await context.CarDTOs.FindAsync(carId);

            if (existingCar != null)
            {
                existingCar.RentBy = null;
                context.Update(existingCar);

                await context.SaveChangesAsync();
                return;
            }

            throw new KeyNotFoundException($"Car with id:{carId} not found!");
        }

        public async Task MakeCarBooked(int carId, int userId)
        {
            using var context = CreateContext();

            var existingCar = await context.CarDTOs.FindAsync(carId);

            if (existingCar != null)
            {
                existingCar.RentBy = userId;
                context.Update(existingCar);
                
                await context.SaveChangesAsync();
                return;
            }

            throw new KeyNotFoundException($"Car with id:{carId} not found!");
        }

        private CarCatalogContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarCatalogContext>();

            return context;
        }
    }
}
