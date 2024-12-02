using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthDAL
{
    public static class Setup
    {
        public static void SetupDALServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuthContext>(options =>
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

            services.AddSingleton<IAuthRepository, AuthRepository>()
                .AddSingleton<ITokenRepository, TokenRepository>()
                .AddSingleton<IUserRepository, UserRepository>();
        }
    }
}
