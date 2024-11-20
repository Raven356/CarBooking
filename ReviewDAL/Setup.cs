using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReviewDAL.Context;
using ReviewDAL.Interfaces;
using ReviewDAL.Repositories;
using System.Reflection;

namespace ReviewDAL
{
    public static class Setup
    {
        public static void SetupDALServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ReviewContext>(options =>
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

            services.AddSingleton<IReviewRepository, ReviewRepository>();
        }
    }
}
