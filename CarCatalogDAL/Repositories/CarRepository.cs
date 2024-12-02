using CarBookingDAL.Context;
using CarBookingDAL.Interfaces;
using CarBookingDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingDAL.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarCatalogContext context;

        public CarRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<CarCatalogContext>();
        }

        public IEnumerable<CarDTO> GetAll()
        {
            var cars = context.CarDTOs
                .Where(car => car.RentBy == null)
                .Include(car => car.CarType)
                .Include(car => car.Model);

            return cars;
        }

        public async Task<CarDTO> GetCarByIdAsync(int carId)
        {
            var car = await context.CarDTOs
                .Include(car => car.CarType)
                .Include(car => car.Model)
                .FirstAsync(car => car.Id == carId);

            return car;
        }

        public async Task MakeCarAvailable(int carId)
        {
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
    }
}
