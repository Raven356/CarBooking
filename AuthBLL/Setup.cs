using AuthBLL.Interfaces;
using AuthBLL.Services;
using AuthDAL.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AuthBLL
{
    public static class Setup
    {
        public static void SetupBLLServices(IServiceCollection services, string connectionString)
        {
            AuthDAL.Setup.SetupDALServices(services, connectionString);

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITokenService, TokenService>();
        }

        public static void SeedDatabase(AuthContext context)
        {
            AuthContextSeeder.SeedData(context);
        }
    }
}
