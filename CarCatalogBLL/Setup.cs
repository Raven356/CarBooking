using CarBookingBLL.Interfaces;
using CarBookingBLL.Services;
using CarBookingDAL.Context;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingBLL
{
    public static class Setup
    {
        public static void SetupBLLServices(IServiceCollection serviceCollection, string connectionString)
        {
            CarBookingDAL.Setup.SetupDALServices(serviceCollection, connectionString);

            serviceCollection.AddSingleton<ICarService, CarService>();
        }

        public static void SeedDatabase(CarCatalogContext context)
        {
            CarBookingContextSeeder.SeedData(context);
        }
    }
}
