using Microsoft.Extensions.DependencyInjection;
using OrderBLL.Interfaces;
using OrderBLL.Services;
using OrderDAL.Context;

namespace OrderBLL
{
    public static class Setup
    {
        public static void SetupBLLServices(IServiceCollection services, string connectionString)
        {
            OrderDAL.Setup.SetupDALServices(services, connectionString);

            services.AddSingleton<IOrderService, OrderService>();
        }

        public static void SeedDatabase(OrderContext context)
        {
            OrderContextSeeder.SeedData(context);
        }
    }
}
