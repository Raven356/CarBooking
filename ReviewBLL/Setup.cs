using Microsoft.Extensions.DependencyInjection;
using ReviewBLL.Interfaces;
using ReviewBLL.Services;
using ReviewDAL.Context;

namespace ReviewBLL
{
    public static class Setup
    {
        public static void SeedDatabase(ReviewContext context)
        {
            ReviewContextSeeder.SeedData(context);
        }

        public static void SetupBLLServices(IServiceCollection services, string connectionString)
        {
            ReviewDAL.Setup.SetupDALServices(services, connectionString);

            services.AddSingleton<IReviewService, ReviewService>();
        }
    }
}
