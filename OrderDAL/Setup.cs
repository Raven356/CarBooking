using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDAL.Context;
using OrderDAL.Interfaces;
using OrderDAL.Repositories;
using System.Reflection;

namespace OrderDAL
{
    public static class Setup
    {
        public static void SetupDALServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<OrderContext>(options =>
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

            services.AddSingleton<IOrderRepository, OrderRepository>();
        }
    }
}
