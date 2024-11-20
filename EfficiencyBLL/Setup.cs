using EfficiencyBLL.Interfaces;
using EfficiencyBLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EfficiencyBLL
{
    public static class Setup
    {
        public static void SetupBLLServices(IServiceCollection services)
        {
            services.AddSingleton<IEfficiencyService, EfficiencyService>();
        }
    }
}
