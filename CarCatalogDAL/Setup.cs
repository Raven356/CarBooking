using CarBookingDAL.Context;
using CarBookingDAL.Interfaces;
using CarBookingDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CarBookingDAL
{
    public static class Setup
    {
        public static void SetupDALServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CarCatalogContext>(options =>
            {
                options.UseSqlServer(connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(
                        typeof(Setup).GetTypeInfo().Assembly.GetName().Name);

                    //Configuring Connection Resiliency:
                    sqlOptions.
                        EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });

            services.AddSingleton<ICarRepository, CarRepository>();
        }
    }
}
