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
                .Include(car => car.CarType)
                .Include(car => car.Model);

            return cars;
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
